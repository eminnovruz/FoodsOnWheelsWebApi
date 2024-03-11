using Application.Models.DTOs.Category;
using Application.Models.DTOs.Food;
using Application.Models.DTOs.Order;
using Application.Models.DTOs.Restaurant;
using Application.Repositories;
using Application.Services.IHelperServices;
using Application.Services.IUserServices;
using Azure.Core;
using Domain.Models;

namespace Infrastructure.Services.UserServices
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBlobService _blobSerice;

        public RestaurantService(IUnitOfWork unitOfWork, IBlobService blobSerice)
        {
            _unitOfWork = unitOfWork;
            _blobSerice = blobSerice;
        }



        #region ADD METOD


        public async Task<bool> AddCategory(AddCategoryRequest request)
        {
            if (request is null)
                throw new ArgumentNullException("The Information Is Not Complete");

            var categoryTest = _unitOfWork.ReadCategoryRepository.GetWhere(x => x.CategoryName.ToLower() == request.CategoryName.ToLower()).ToList();
            if (categoryTest.Count > 0)
                throw new InvalidDataException("A Category With This Name Already Exists");

            var category = new Category
            {
                Id = Guid.NewGuid().ToString(),
                CategoryName = request.CategoryName,
                FoodIds = request.FoodIds,
            };


            if (request.FoodIds.Count != 0)
            {
                var foods = await _unitOfWork.ReadFoodRepository.GetAllAsync();

                foreach (var item in request.FoodIds)
                {
                    var food = foods.FirstOrDefault(x => x?.Id == item); 
                   if (food == default)
                        throw new ArgumentNullException("Food Id Is Not Found");
                   
                    food.CategoryIds.Add(category.Id);
                    
                    await _unitOfWork.WriteFoodRepository.UpdateAsync(food.Id);
                    await _unitOfWork.WriteFoodRepository.SaveChangesAsync();
                }
            }

            await _unitOfWork.WriteCategoryRepository.AddAsync(category);
            await _unitOfWork.WriteCategoryRepository.SaveChangesAsync();

            return true;
        }


        public async Task<bool> AddFood(AddFoodRequest request)
        {
            if (request is null)
                throw new ArgumentNullException("The Information Is Not Complete");

            var restaurant = await _unitOfWork.ReadRestaurantRepository.GetAsync(request.RestaurantId);
            if (restaurant is null)
                throw new ArgumentNullException("Wrong Restaurant");

            var food = new Food
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Name,
                Description = request.Description,
                CategoryIds = request.CategoryIds,
                Price = request.Price,
                RestaurantId = request.RestaurantId
            };


            var form = request.File;
            using (var stream = form.OpenReadStream())
            {
                var fileName = food.Id + "-" + food.Name + ".jpg";
                var contentType = form.ContentType;


                var blobResult = await _blobSerice.UploadFileAsync(stream, fileName, contentType);
                if (blobResult is false)
                {
                    return false;
                }

                food.ImageUrl = _blobSerice.GetSignedUrl(fileName);
            }

            var categorys = await _unitOfWork.ReadCategoryRepository.GetAllAsync();
            foreach (var item in request.CategoryIds)
            {
                var category = categorys.FirstOrDefault(x => item == x.Id);
                if (category is null)
                    throw new ArgumentNullException("Category Id Is Not Found");
                category.FoodIds.Add(food.Id);
                await _unitOfWork.WriteCategoryRepository.UpdateAsync(category.Id);
                await _unitOfWork.WriteCategoryRepository.SaveChangesAsync();
            }

            restaurant.FoodIds.Add(food.Id);
            await _unitOfWork.WriteRestaurantRepository.UpdateAsync(restaurant.Id);
            await _unitOfWork.WriteRestaurantRepository.SaveChangesAsync();


            await _unitOfWork.WriteFoodRepository.AddAsync(food);
            await _unitOfWork.WriteFoodRepository.SaveChangesAsync();
            return true;
        }
        

        #endregion


        #region GET METOD


        public async Task<RestaurantInfoDto> GetRestaurantInfo(string Id)
        {
            var restaurant = await _unitOfWork.ReadRestaurantRepository.GetAsync(Id);
            if (restaurant is null)
                throw new ArgumentNullException("Wrong Restaurant");

            RestaurantInfoDto restaurantDto = new RestaurantInfoDto
            {
                Id = Id,
                Name = restaurant.Name,
                Description = restaurant.Description,
                FoodIds = restaurant.FoodIds,
                ImageUrl = restaurant.ImageUrl,
                Rating = restaurant.Rating,
            };


            return restaurantDto;
        }


        public List<OrderInfoDto> GetActiveOrders(string Id)
        {
            var orders = _unitOfWork.ReadOrderRepository.GetWhere(x => x.RestaurantId == Id).ToList();
            if (orders.Count == 0)
                throw new InvalidDataException("There are no orders");


            var activeOrders = new List<OrderInfoDto>();
            foreach (var order in orders)
            {
                if (order is not null && order.OrderFinishTime == default)
                {
                    activeOrders.Add(new OrderInfoDto
                    {
                        Id = order.Id,
                        RestaurantId = order.RestaurantId,
                        OrderDate = order.OrderDate,
                        PayedWithCard = order.PayedWithCard,
                        FoodIds = order.OrderedFoodIds,
                        UserId = order.UserId,
                        Rate = order.Amount,
                    });
                }
            }
            if (activeOrders.Count == 0)
                throw new ArgumentNullException("There are no ongoing orders at the moment");

            return activeOrders;
        }


        public List<OrderInfoDto> GetOrderHistory(string Id)
        {
            var orders = _unitOfWork.ReadOrderRepository.GetWhere(x => x.RestaurantId == Id).ToList();
            if (orders.Count == 0)
                throw new InvalidDataException("There are no orders");

            var restaurantOrders = new List<OrderInfoDto>();
            foreach (var order in orders)
            {
                if (order is not null)
                {
                    restaurantOrders.Add(new OrderInfoDto
                    {
                        Id = order.Id,
                        RestaurantId = order.RestaurantId,
                        OrderDate = order.OrderDate,
                        PayedWithCard = order.PayedWithCard,
                        FoodIds = order.OrderedFoodIds,
                        UserId = order.UserId,
                        Rate = order.Amount,
                    });
                }
            }

            if (restaurantOrders.Count == 0)
                throw new ArgumentNullException("There are no orders");

            return restaurantOrders;
        }


        public List<OrderInfoDto> GetPastOrderInfoById(string Id)
        {
            var orders = _unitOfWork.ReadOrderRepository.GetWhere(x => x.RestaurantId == Id).ToList();
            if (orders.Count == 0)
                throw new InvalidDataException("There are no orders");


            var pastOrders = new List<OrderInfoDto>();
            foreach (var order in orders)
            {
                if (order is not null && order.OrderFinishTime != default)
                {
                    pastOrders.Add(new OrderInfoDto
                    {
                        Id = order.Id,
                        RestaurantId = order.RestaurantId,
                        OrderDate = order.OrderDate,
                        PayedWithCard = order.PayedWithCard,
                        FoodIds = order.OrderedFoodIds,
                        UserId = order.UserId,
                        Rate = order.Amount,
                    });
                }
            }
            if (pastOrders.Count == 0)
                throw new ArgumentNullException("There are no past orders");

            return pastOrders;
        }


        #endregion


        #region DELETE METOD


        public async Task<bool> RemoveFood(string Id)
        {
            var food = await _unitOfWork.ReadFoodRepository.GetAsync(Id);
            if (food is null)
                throw new ArgumentNullException("Wrong Food");

            var restaurant = await _unitOfWork.ReadRestaurantRepository.GetAsync(food.RestaurantId);
            if (restaurant is null)
                throw new ArgumentNullException("Wrong Restaurant");


            var categorys = await _unitOfWork.ReadCategoryRepository.GetAllAsync();
            foreach (var item in food.CategoryIds)
            {
                var category = categorys.FirstOrDefault(x => item == x.Id);
                if (category is null)
                    throw new ArgumentNullException("Category Id Is Not Found");
                category.FoodIds.Remove(food.Id);
                await _unitOfWork.WriteCategoryRepository.UpdateAsync(category.Id);
                await _unitOfWork.WriteCategoryRepository.SaveChangesAsync();
            }

            await _blobSerice.DeleteFileAsync(food.Id + "-" + food.Name + ".jpg");

            restaurant.FoodIds.Remove(food.Id);
            await _unitOfWork.WriteRestaurantRepository.UpdateAsync(restaurant.Id);
            await _unitOfWork.WriteRestaurantRepository.SaveChangesAsync();


            await _unitOfWork.WriteFoodRepository.RemoveAsync(food.Id);
            await _unitOfWork.WriteFoodRepository.SaveChangesAsync();
            return true;
        }


        #endregion      
    }
}
