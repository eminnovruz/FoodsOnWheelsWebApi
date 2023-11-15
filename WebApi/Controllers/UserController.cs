using Application.Models.DTOs.Category;
using Application.Models.DTOs.Order;
using Application.Models.DTOs.Restaurant;
using Application.Services;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Persistence.Context;
using Serilog;

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
        public async Task<ActionResult<IEnumerable<RestaurantInfoDto>>> GetAllRestaurants()
        {
            try
            {
                return Ok(await _userService.GetAllRestaurants());
            }
            catch (Exception exception)
            {
                Log.Error("Error occured on [GET] GetAllRestaurants");
                return BadRequest(exception.Message); 
            }
        }

        [HttpGet("getAllCategories")]
        public async Task<ActionResult<IEnumerable<CategoryInfoDto>>> GetAllCategories()
        {
            try
            {
                return Ok(await _userService.GetAllFoodCategories());
            }
            catch (Exception exception)
            {
                Log.Error("Error occured on [GET] GetAllCategories");
                return BadRequest(exception.Message);
            }
        }

        //[HttpGet("rateOrder")]
        //public Task<bool> RateOrder(RateOrderDto request)
        //{

        //    return _userService.RateOrder(request);
        //}
    }
}
