using Application.Models.DTOs.Category;
using Application.Models.DTOs.Food;
using Application.Models.DTOs.Order;
using Application.Models.DTOs.Restaurant;
using Application.Models.DTOs.User;
using Application.Services.IUserServices;
using Azure.Core;
using Infrastructure.Services.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Restaurant")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }


        #region ADD

        [HttpPost("addCategory")]
        public async Task<ActionResult<bool>> AddCategory([FromBody] AddCategoryRequest request)
        {
            try
            {
                return Ok(await _restaurantService.AddCategory(request));
            }
            catch (Exception exception)
            {
                Log.Error($"Error occured on [POST] AddCategory {exception.Message}");
                return BadRequest(exception.Message);
            }
        }


        [HttpPost("addFood")]
        public async Task<ActionResult<bool>> AddFood([FromForm] AddFoodRequest request)
        {
            try
            {
                return Ok(await _restaurantService.AddFood(request));
            }
            catch (Exception exception)
            {
                Log.Error($"Error occured on [POST] AddFood : {exception.Message}");
                return BadRequest(exception.Message);
            }
        }


        [HttpPost("inLastDecidesSituation")]
        public async Task<ActionResult<bool>> InLastDecidesSituation([FromBody] InLastSituationOrderDto orderDto)
        {
            try
            {
                return Ok(await _restaurantService.InLastDecidesSituation(orderDto));
            }
            catch (Exception exception)
            {
                Log.Error($"Error occured on [POST] InLastDecidesSituation : {exception.Message}");
                return BadRequest(exception.Message);
            }
        }

        #endregion


        #region UPDATE

        [HttpPut("updateProfile")]
        public async Task<ActionResult<bool>> UpdateProfile([FromBody]UpdateRestaurantDto dto)
        {
            try
            {
                return Ok(await _restaurantService.UpdateProfile(dto));
            }
            catch (Exception exception)
            {
                Log.Error($"Error occured on [PUT] UpdateProfile : {exception.Message}");
                return BadRequest(exception.Message);
            }
        }


        [HttpPut("updateProfilePasssword")]
        public async Task<ActionResult<bool>> UpdateProfilePasssword([FromBody] UpdateUserPasswordDto dto)
        {
            try
            {
                return Ok(await _restaurantService.UpdateProfilePasssword(dto));
            }
            catch (Exception exception)
            {
                Log.Error($"Error occured on [PUT] UpdateProfilePasssword");
                return BadRequest(exception.Message);
            }
        }


        [HttpPut("updateFood")]
        public async Task<ActionResult<bool>> UpdateFood([FromForm] UpdateFoodRequest request)
        {
            try
            {
                var result = await _restaurantService.UpdateFood(request);
                return Ok(result);
            }
            catch (Exception exception)
            {
                Log.Error("Error occurred on [PUT] UpdateFood");
                return BadRequest(exception.Message);
            }
        }


        [HttpPost("updateStatusOrder")]
        public async Task<ActionResult<bool>> UpdateStatusOrder(UpdateOrderStatusDto statusDto)
        {
            try
            {
                return Ok(await _restaurantService.UpdateStatusOrder(statusDto));
            }
            catch (Exception exception)
            {
                Log.Error($"Error occured on [POST] UpdateStatusOrder : {exception.Message}");
                return BadRequest(exception.Message);
            }
        }

        #endregion


        #region GET 

        [HttpGet("getRestaurantInfo")]
        public async Task<ActionResult<RestaurantInfoDto>> GetRestaurantInfo([FromQuery] string Id)
        {
            try
            {
                return Ok(await _restaurantService.GetProfileInfo(Id));
            }
            catch (Exception exception)
            {
                Log.Error($"Error occured on [GET] GetRestaurantInfo : {exception.Message}");
                return BadRequest(exception.Message);
            }
        }


        [HttpGet("getActiveOrders")]
        public ActionResult<List<InfoOrderDto>> GetActiveOrders([FromQuery] string Id)
        {
            try
            {
                return Ok(_restaurantService.GetActiveOrders(Id));
            }
            catch (Exception exception)
            {
                Log.Error($"Error occured on [GET] GetActiveOrders : {exception.Message}");
                return BadRequest(exception.Message);
            }
        }


        [HttpGet("getAllOrders")]
        public async Task<ActionResult<IEnumerable<InfoOrderDto>>> GetAllOrders([FromQuery] string resturantId)
        {
            try
            {
                return Ok(await _restaurantService.GetAllOrders(resturantId));
            }
            catch (Exception exception)
            {
                Log.Error($"Error occured on [GET] GetAllOrders : {exception.Message}");
                return BadRequest(exception.Message);
            }
        }


        [HttpGet("getOrderHistory")]
        public ActionResult<List<InfoOrderDto>> GetOrderHistory([FromQuery] string Id)
        {
            try
            {
                return Ok(_restaurantService.GetOrderHistory(Id));
            }
            catch (Exception exception)
            {
                Log.Error($"Error occured on [GET] GetOrderHistory : {exception.Message}");
                return BadRequest(exception.Message);
            }
        }


        [HttpGet("getPastOrderInfoById")]
        public ActionResult<List<InfoOrderDto>> GetPastOrderInfoById([FromBody] string Id)
        {
            try
            {
                return Ok(_restaurantService.GetPastOrderInfoById(Id));
            }
            catch (Exception exception)
            {
                Log.Error($"Error occured on [GET] GetPastOrderInfoById : {exception.Message}");
                return BadRequest(exception.Message);
            }
        }


        [HttpGet("waitingOrders")]
        public ActionResult<IEnumerable<InfoOrderDto>> WaitingOrders([FromBody] string resturantId)
        {
            try
            {
                return Ok(_restaurantService.WaitingOrders(resturantId));
            }
            catch (Exception exception)
            {
                Log.Error($"Error occured on [GET] WaitingOrders : {exception.Message}");
                return BadRequest(exception.Message);
            }
        }


        #endregion


        #region REMOVE

        [HttpDelete("RemoveProfile")]
        public async Task<ActionResult<bool>> RemoveProfile(string restaurantId)
        {
            try
            {
                return Ok(await _restaurantService.RemoveProfile(restaurantId));
            }
            catch (Exception exception)
            {
                Log.Error($"Error occured on [DELETE] RemoveProfile");
                return BadRequest(exception.Message);
            }
        }


        [HttpDelete("removeFood")]
        public async Task<ActionResult<bool>> RemoveFood(string Id)
        {
            try
            {
                return Ok(await _restaurantService.RemoveFood(Id));
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
