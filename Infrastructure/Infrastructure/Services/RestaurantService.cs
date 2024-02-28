using Application.Repositories;
using Application.Services;
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

        public Task<bool> AddCategory()
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddFood()
        {
            throw new NotImplementedException();
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

        public Task<bool> GetRestaurantInfo()
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveCategory()
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveFood()
        {
            throw new NotImplementedException();
        }
    }
}
