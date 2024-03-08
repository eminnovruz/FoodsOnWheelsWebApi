using Application.Models.DTOs.Category;
using Application.Models.DTOs.Food;
using Application.Models.DTOs.Order;
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
                Log.Error($"Error occured on [POST] AddCategory {exception.Message}");
                return BadRequest(exception.Message);
            }
        }


        [HttpPost("addFood")]
        public async Task<ActionResult<bool>> AddFood([FromForm]AddFoodRequest request)
        {
            try
            {
                return Ok(await restaurantService.AddFood(request));
            }
            catch (Exception exception)
            {
                Log.Error($"Error occured on [POST] AddFood : {exception.Message}");
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
                Log.Error($"Error occured on [GET] GetRestaurantInfo : {exception.Message}");
                return BadRequest(exception.Message);
            }
        }


        [HttpGet("getActiveOrders")]
        public ActionResult<List<OrderInfoDto>> GetActiveOrders([FromQuery]string Id)
        {
            try
            {
                return Ok(restaurantService.GetActiveOrders(Id));
            }
            catch (Exception exception)
            {
                Log.Error($"Error occured on [GET] GetActiveOrders : {exception.Message}");
                return BadRequest(exception.Message);
            }

        }


        [HttpGet("getOrderHistory")]
        public ActionResult<List<OrderInfoDto>> GetOrderHistory(string Id)
        {
            try
            {
                return Ok(restaurantService.GetOrderHistory(Id));
            }
            catch (Exception exception)
            {
                Log.Error($"Error occured on [GET] GetOrderHistory : {exception.Message}");
                return BadRequest(exception.Message);
            }
        }


        [HttpGet("getPastOrderInfoById")]
        public ActionResult<List<OrderInfoDto>> GetPastOrderInfoById(string Id)
        {
            try
            {
                return Ok(restaurantService.GetPastOrderInfoById(Id));
            }
            catch (Exception exception)
            {
                Log.Error($"Error occured on [GET] GetPastOrderInfoById : {exception.Message}");
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
                Log.Error($"Error occured on [DELETE] RemoveFood : {exception.Message}");
                return BadRequest(exception.Message);
            }
        }


        #endregion
    }
}
