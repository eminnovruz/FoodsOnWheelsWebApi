using Application.Models.DTOs.Category;
using Application.Models.DTOs.Restaurant;
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


    }
}
