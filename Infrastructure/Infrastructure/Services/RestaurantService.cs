using Application.Models.DTOs.Category;
using Application.Models.DTOs.Food;
using Application.Models.DTOs.Restaurant;
using Application.Repositories;
using Application.Services;
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

            var categoryTest = _unitOfWork.ReadCategoryRepository.GetWhere(x => x.CategoryName.ToLower() == request.CategoryName.ToLower());
            if (categoryTest is not null)
                throw new InvalidDataException("A Category With This Name Already Exists");

            var category = new Category
            {
                Id = Guid.NewGuid().ToString(),
                CategoryName = request.CategoryName,
                FoodIds = request.FoodIds,
            };

            await _unitOfWork.WriteCategoryRepository.AddAsync(category);
            await _unitOfWork.WriteRestaurantRepository.SaveChangesAsync();

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
                var fileName = Guid.NewGuid().ToString() + "-" + food.Name + ".jpg";
                var contentType = form.ContentType;

                var blobResult = _blobSerice.UploadFile(stream, fileName, contentType);
                if (blobResult is false)
                {
                    return false;
                }

                food.ImageUrl = _blobSerice.GetSignedUrl(fileName);
            }

            restaurant.FoodIds.Add(food.Id);

            _unitOfWork.WriteRestaurantRepository.Update(restaurant);
            await _unitOfWork.WriteFoodRepository.AddAsync(food);

            await _unitOfWork.WriteRestaurantRepository.SaveChangesAsync();
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


        public Task<bool> GetActiveOrders()
        {
            throw new NotImplementedException();
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


        public Task<bool> RemoveCategory()
        {
            throw new NotImplementedException();
        }


        public async Task<bool> RemoveFood(string Id)
        {
            var food = await _unitOfWork.ReadFoodRepository.GetAsync(Id);
            if (food is null)
                throw new ArgumentNullException("Wrong food");


            _unitOfWork.WriteFoodRepository.Remove(food);
            await _unitOfWork.WriteFoodRepository.SaveChangesAsync();
            return true;
        }

        #endregion      
    }
}
