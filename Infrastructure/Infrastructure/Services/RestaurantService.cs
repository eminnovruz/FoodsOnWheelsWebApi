using Application.Models.DTOs.Category;
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

        public RestaurantService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        #region ADD METOD

        public async Task<bool> AddCategory(AddCategoryRequest request)
        {
            if (request is null)
                throw new ArgumentNullException();

            var categoryTest = _unitOfWork.ReadCategoryRepository.GetWhere(x => x.CategoryName.ToLower() == request.CategoryName.ToLower());
            if (categoryTest is not null)
                throw new InvalidDataException("");

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

        public Task<bool> AddFood()
        {
            throw new NotImplementedException();
        }

        #endregion


        #region GET METOD


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


        public Task<bool> GetRestaurantInfo()
        {
            throw new NotImplementedException();
        }

        #endregion


        #region DELETE METOD


        public Task<bool> RemoveCategory()
        {
            throw new NotImplementedException();
        }


        public Task<bool> RemoveFood()
        {
            throw new NotImplementedException();
        }
        #endregion      
    }
}
