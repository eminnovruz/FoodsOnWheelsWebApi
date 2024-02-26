using Application.Models.DTOs.Category;
using Application.Models.DTOs.Food;
using Application.Models.DTOs.Restaurant;
using Application.Models.DTOs.User;
using Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IUserService _userService;

        public ClientController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("getAllRestaurants")]
        public ActionResult<RestaurantInfoDto> GetAllRestaurants()
        {
            try
            {
                return Ok( _userService.GetAllRestaurants());
            }
            catch (Exception exception)
            {
                Log.Error("Error occured on [GET] GetAllRestaurants");
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("getAllCategories")]
        public ActionResult<CategoryInfoDto> GetAllCategories()
        {
            try
            {
                return Ok(_userService.GetAllFoodCategories());
            }
            catch (Exception exception)
            {
                Log.Error("Error occured on [GET] GetAllRestaurants");
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("getFoodsByCategory")]
        public ActionResult<IEnumerable<FoodInfoDto>> GetFoodsByCategory(string categoryId)
        {
            try
            {
                return Ok(_userService.GetFoodsByCategory(categoryId));
            }
            catch (Exception exception)
            {
                Log.Error("Error occured on [GET] GetFoodsByCategory");
                return BadRequest(exception.Message);
            }
        }


        [HttpGet("getFoodsByRestaurant")]
        public async Task<ActionResult<IEnumerable<FoodInfoDto>>> GetFoodsByRestaurant(string restaurantId)
        {
            try
            {
                return Ok(await _userService.GetFoodsByRestaurant(restaurantId));
                
            }
            catch (Exception exception)
            {
                Log.Error("Error occured on [GET] GetFoodsByRestaurant");
                return BadRequest(exception.Message);
            }
        }


        [HttpGet("getProfileInfo")]
        public async Task<ActionResult<GetUserProfileInfoDto>> GetProfileInfo(string userId)
        {
            try
            {
                return Ok(await _userService.GetProfileInfo(userId));
            }
            catch (Exception exception)
            {
                Log.Error("Error occured on [GET] GetProfileInfo");
                return BadRequest(exception.Message);
            }
        }
    }
}
