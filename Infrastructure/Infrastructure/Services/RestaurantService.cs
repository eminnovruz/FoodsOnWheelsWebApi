using Application.Models.DTOs.Category;
using Application.Models.DTOs.Food;
using Application.Models.DTOs.Order;
using Application.Models.DTOs.Restaurant;
using Application.Repositories;
using Application.Services;
using Azure.Storage.Blobs.Models;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
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

            await _unitOfWork.WriteCategoryRepository.AddAsync(category);
            await _unitOfWork.WriteCategoryRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddFood(AddFoodToRestaurantDto request)
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

            restaurant.FoodIds.Add(food.Id);

            _unitOfWork.WriteRestaurantRepository.Update(restaurant);
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

            var restaurantDto = new RestaurantInfoDto
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


        public async Task<List<OrderInfoDto>> GetActiveOrders(string Id)
        {
            var restaurant = await _unitOfWork.ReadRestaurantRepository.GetAsync(Id);
            if (restaurant is null)
                throw new ArgumentNullException("Wrong Restaurant");

            var orders = _unitOfWork.ReadOrderRepository.GetWhere(x=> x.RestaurantId == Id).ToList();
            if (orders.Count == 0)
                throw new InvalidDataException("There are no orders");
            

            var activeOrders = new List<OrderInfoDto>();
            foreach (var order in orders)
            {
                if (order is not null && order.OrderFinishTime == default)
                {
                    activeOrders.Add(new OrderInfoDto
                    {
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


        public Task<bool> GetOrderHistory()
        {
            throw new NotImplementedException();
        }


        public Task<bool> GetPastOrderInfoById()
        {
            throw new NotImplementedException();
        }



        #endregion


        #region DELETE METOD

        public async Task<bool> RemoveFood(string Id)
        {
            var food = await _unitOfWork.ReadFoodRepository.GetAsync(Id);
            if (food is null)
                throw new ArgumentNullException("Wrong food");

            await _blobSerice.DeleteFileAsync(food.Id + "-" + food.Name + ".jpg");

            _unitOfWork.WriteFoodRepository.Remove(food);
            await _unitOfWork.WriteFoodRepository.SaveChangesAsync();
            return true;
        }

        #endregion      
    }
}
