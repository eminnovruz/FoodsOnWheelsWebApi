using Application.Models.DTOs.Category;
using Application.Models.DTOs.Food;
using Application.Models.DTOs.Restaurant;
using Application.Services.IUserServices;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {

        private readonly IRestaurantService restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            this.restaurantService = restaurantService;
        }


        #region ADD METOD
        [HttpPost("addCategory")]
        public async Task<ActionResult<bool>> AddCategory(AddCategoryRequest request) 
        {
            try
            {
                return Ok(await restaurantService.AddCategory(request));
            }
            catch (Exception exception)
            {
                Log.Error("Error occured on [POST] AddCategory");
                return BadRequest(exception.Message);
            }
        }


        [HttpPost("addFood")]
        public async Task<ActionResult<bool>> AddFood([FromForm]AddFoodToRestaurantDto request)
        {
            try
            {
                return Ok(await restaurantService.AddFood(request));
            }
            catch (Exception exception)
            {
                Log.Error("Error occured on [POST] AddFood");
                return BadRequest(exception.Message);
            }
        }

        #endregion


        #region GET METOD
        [HttpGet("getRestaurantInfo")]
        public async Task<ActionResult<RestaurantInfoDto>> GetRestaurantInfo([FromQuery]string Id)
        {
            try
            {
                return Ok(await restaurantService.GetRestaurantInfo(Id));
            }
            catch (Exception exception)
            {
                Log.Error("Error occured on [GET] GetRestaurantInfo");
                return BadRequest(exception.Message);
            }
        }

        #endregion


        #region DELETE METOD
        [HttpDelete("removeFood")]
        public async Task<ActionResult<bool>> RemoveFood(string Id)
        {
            try
            {
                return Ok(await restaurantService.RemoveFood(Id));
            }
            catch (Exception exception)
            {
                Log.Error("Error occured on [DELETE] RemoveFood");
                return BadRequest(exception.Message);
            }
        }
        #endregion
    }
}
