using Application.Models.DTOs.Category;
using Application.Models.DTOs.Order;
using Application.Models.DTOs.Restaurant;
using Application.Services;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Persistence.Context;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            this._userService = userService;
        }

        [HttpGet("getAllRestaurants")]
        public Task<IEnumerable<RestaurantInfoDto>> GetAllRestaurants()
        {
            return _userService.GetAllRestaurants();
        }

        [HttpGet("getAllCategories")]
        public Task<IEnumerable<CategoryInfoDto>> GetAllCategories()
        {
            return _userService.GetAllFoodCategories();
        }

        [HttpGet("rateOrder")]
        public Task<bool> RateOrder(RateOrderDto request)
        {
            return _userService.RateOrder(request);
        }
    }
}
