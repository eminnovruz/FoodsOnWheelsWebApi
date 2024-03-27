using Application.Models.DTOs.Category;
using Application.Models.DTOs.Food;
using Application.Models.DTOs.Order;
using Application.Models.DTOs.Restaurant;
using Application.Repositories;
using Application.Services.IHelperServices;
using Application.Services.IUserServices;
using Azure.Core;
using Domain.Models;
using Domain.Models.Enums;
using System.Linq;

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

        public async Task<bool> AddCategory(AddCategoryRequest request)
        {
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
                var foods =_unitOfWork.ReadFoodRepository.GetAll().ToList();
                if (foods is null || foods.Count == 0)
                    throw new ArgumentNullException("Foods Is Not Found");

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
                    return false;

                food.ImageUrl = _blobSerice.GetSignedUrl(fileName);
            }

            var categorys = _unitOfWork.ReadCategoryRepository.GetAll().ToList();
            if (categorys is null || categorys.Count == 0)
                throw new ArgumentNullException("Categorys Is Not Found");
            foreach (var item in request.CategoryIds)
            {
                var category = categorys.FirstOrDefault(x => item == x?.Id);
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
        
        public async Task<bool> InLastDecidesSituation(InLastSituationOrderDto orderDto)
        {
            var order = await _unitOfWork.ReadOrderRepository.GetAsync(orderDto.OrderId);
            if (order is null)
                throw new ArgumentNullException("There are no orders");

            if (orderDto.IsLastSituation)
                order.OrderStatus = OrderStatus.Comfirmed;
            else
                order.OrderStatus = OrderStatus.Rejected;

            await _unitOfWork.WriteOrderRepository.UpdateAsync(order.Id);
            await _unitOfWork.WriteOrderRepository.SaveChangesAsync();

            return true;
        }

        public IEnumerable<InfoOrderDto> WaitingOrders(string resturantId)
        {
            var orders = _unitOfWork.ReadOrderRepository.GetWhere(x=> x.RestaurantId == resturantId && x.OrderStatus == OrderStatus.Waiting).ToList();
            if (orders.Count == 0)
                throw new ArgumentNullException("There are no orders");

            var waitingOrdersDto = new List<InfoOrderDto>();

            foreach (var item in orders)
            {
                if (item is not null)
                {
                    waitingOrdersDto.Add(new InfoOrderDto
                    {
                        Id = item.Id,
                        OrderStatus = item.OrderStatus,
                        OrderDate = item.OrderDate,
                        FoodIds = item.OrderedFoodIds,
                        PayedWithCard = item.PayedWithCard,
                        Rate = 0,
                        Amount = item.Amount,
                        UserId = item.UserId,
                        RestaurantId = item.RestaurantId,
                    });
                }
            }

            return waitingOrdersDto;
        }

        public async Task<IEnumerable<InfoOrderDto>> GetAllOrders(string resturantId)
        {
            var orders = _unitOfWork.ReadOrderRepository.GetWhere(x => x.RestaurantId == resturantId).ToList();
            if (orders.Count == 0)
                throw new InvalidDataException("There are no orders");

            var ordersDto = new List<InfoOrderDto>();
            foreach (var order in orders)
            {
                if (order is not null)
                {
                    var rateOrder = await _unitOfWork.ReadOrderRatingRepository.GetAsync(order.OrderRatingId);
                    byte rating = 0;
                    if (rateOrder is not null)
                        rating = rateOrder.Rate;
                    
                    ordersDto.Add(new InfoOrderDto
                    {
                        Id = order.Id,
                        RestaurantId = order.RestaurantId,
                        OrderDate = order.OrderDate,
                        PayedWithCard = order.PayedWithCard,
                        FoodIds = order.OrderedFoodIds,
                        UserId = order.UserId,
                        Amount = order.Amount,
                        OrderStatus = order.OrderStatus,
                        Rate = rating,
                    });
                }
            }
            if (orders.Count == 0)
                throw new ArgumentNullException("There are orders at the moment");

            return ordersDto;
        }

        public async Task<RestaurantInfoDto> GetRestaurantInfo(string Id)
        {
            var restaurant = await _unitOfWork.ReadRestaurantRepository.GetAsync(Id);
            if (restaurant is null)
                throw new ArgumentNullException("Wrong Restaurant");

            await UpdateRestaurantRaiting(Id);

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
        
        public IEnumerable<InfoOrderDto> GetActiveOrders(string Id)
        {
            var orders = _unitOfWork.ReadOrderRepository.GetWhere(x => x.RestaurantId == Id).ToList();
            if (orders.Count == 0)
                throw new InvalidDataException("There are no orders");

            var activeOrders = new List<InfoOrderDto>();
            foreach (var order in orders)
            {
                if (order is not null && order.OrderFinishTime == default)
                {
                    activeOrders.Add(new InfoOrderDto
                    {
                        Id = order.Id,
                        RestaurantId = order.RestaurantId,
                        OrderDate = order.OrderDate,
                        PayedWithCard = order.PayedWithCard,
                        FoodIds = order.OrderedFoodIds,
                        UserId = order.UserId,
                        Amount = order.Amount,
                        OrderStatus = order.OrderStatus
                    });
                }
            }

            if (activeOrders.Count == 0)
                throw new ArgumentNullException("There are no ongoing orders at the moment");

            return activeOrders;
        }

        public async Task<IEnumerable<InfoOrderDto>> GetOrderHistory(string Id)
        {
            var orders = _unitOfWork.ReadOrderRepository.GetWhere(x => x.RestaurantId == Id).ToList();
            if (orders.Count == 0)
                throw new InvalidDataException("There are no orders");

            var restaurantOrders = new List<InfoOrderDto>();
            foreach (var order in orders)
            {
                if (order is not null)
                {
                    var rateOrder = await _unitOfWork.ReadOrderRatingRepository.GetAsync(order.OrderRatingId);
                    restaurantOrders.Add(new InfoOrderDto
                    {
                        Id = order.Id,
                        RestaurantId = order.RestaurantId,
                        OrderDate = order.OrderDate,
                        PayedWithCard = order.PayedWithCard,
                        FoodIds = order.OrderedFoodIds,
                        UserId = order.UserId,
                        Amount = order.Amount,
                        OrderStatus = order.OrderStatus,
                        Rate = rateOrder.Rate
                    });
                }
            }

            if (restaurantOrders.Count == 0)
                throw new ArgumentNullException("There are no orders");

            return restaurantOrders;
        }

        public async Task<InfoOrderDto> GetPastOrderInfoById(string orderId)
        {
            var order = await _unitOfWork.ReadOrderRepository.GetAsync(orderId);
            if (order is null)
                throw new InvalidDataException("There are no order");

            var rateOrder = await _unitOfWork.ReadOrderRatingRepository.GetAsync(order.OrderRatingId);
            if (rateOrder is null)
                throw new InvalidDataException("There are no order");


            var pastOrders = new InfoOrderDto
            {
                Id = order.Id,
                RestaurantId = order.RestaurantId,
                OrderDate = order.OrderDate,
                PayedWithCard = order.PayedWithCard,
                FoodIds = order.OrderedFoodIds,
                UserId = order.UserId,
                Amount = order.Amount,
                OrderStatus = order.OrderStatus,
                Rate = rateOrder.Rate 
            };
          
            return pastOrders;
        }

        public async Task<bool> UpdateStatusOrder(UpdateOrderStatusDto statusDto)
        {
            var order = await _unitOfWork.ReadOrderRepository.GetAsync(statusDto.OrderId);
            if (order is null)
                throw new ArgumentNullException("There are no orders");
            if (order.OrderStatus == OrderStatus.Waiting)
                throw new ArgumentException("");

            order.OrderStatus = statusDto.OrderStatus;

            await _unitOfWork.WriteOrderRepository.UpdateAsync(order.Id);
            await _unitOfWork.WriteOrderRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveFood(string Id)
        {
            var food = await _unitOfWork.ReadFoodRepository.GetAsync(Id);
            if (food is null)
                throw new ArgumentNullException("Wrong Food");

            var restaurant = await _unitOfWork.ReadRestaurantRepository.GetAsync(food.RestaurantId);
            if (restaurant is null)
                throw new ArgumentNullException("Wrong Restaurant");

            var categorys = _unitOfWork.ReadCategoryRepository.GetAll().ToList();
            if (categorys is null || categorys.Count == 0)
                throw new ArgumentNullException("Categorys Is Not Found");
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

        private async Task<bool> UpdateRestaurantRaiting(string resturantId)
        {

            var resturant = await _unitOfWork.ReadRestaurantRepository.GetAsync(resturantId);
            if (resturant is null)
                throw new ArgumentNullException("Resturant not found");

            var comments = _unitOfWork.ReadRestaurantCommentRepository.GetWhere(x => x.RestaurantId == resturantId).ToList();
            var orders = _unitOfWork.ReadOrderRepository.GetWhere(x => x.RestaurantId == resturantId).ToList();
            if (orders.Count == 0)
                return false;

            var ordersRaiting = new List<int>();
            foreach (var order in orders)
            {
                if (order is not null)
                {
                    var orderRaiting = await _unitOfWork.ReadOrderRatingRepository.GetAsync(order.OrderRatingId);
                    if (orderRaiting is not null)
                        ordersRaiting.Add(orderRaiting.Rate);
                }
            }
            var averageOrders = ordersRaiting.Average();
            
            if (comments.Count != 0)
            {
                var averageComments = comments.Average(x => x?.Rating);
                var average =  (averageComments + averageOrders) / 2;
                resturant.Rating = Convert.ToUInt32(average);
            }
            else
            {
                var average = averageOrders;
                resturant.Rating = Convert.ToUInt32(average);
            } 



            var result = await _unitOfWork.WriteRestaurantRepository.UpdateAsync(resturant.Id);
            await _unitOfWork.WriteRestaurantRepository.SaveChangesAsync();

            return result;

        }
    }
}