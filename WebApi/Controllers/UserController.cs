using Application.Models.DTOs.BankCard;
using Application.Models.DTOs.Category;
using Application.Models.DTOs.Food;
using Application.Models.DTOs.Order;
using Application.Models.DTOs.Restaurant;
using Application.Models.DTOs.User;
using Application.Services.IUserServices;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            _userService = userService;
        }

        #region GET
        
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
        public ActionResult<FoodInfoDto> GetFoodsByCategory(string categoryId)
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


        [HttpGet("getUserBankCard")]
        public async Task<ActionResult<GetBankCardDto>> GetUserBankCard(string cardId)
        {
            try
            {
                return Ok(await _userService.GetUserBankCard(cardId));
            }
            catch (Exception exception)
            {
                Log.Error("Error occured on [GET] GetUserBankCard");
                return BadRequest(exception.Message);
            }
        }


        [HttpGet("getAllUserBankCard")]
        public ActionResult<IEnumerable<GetBankCardDto>> GetAllUserBankCard(string userId)
        {
            try
            {
                return Ok(_userService.GetAllUserBankCard(userId));
            }
            catch (Exception exception)
            {
                Log.Error("Error occured on [GET] GetUserBankCard");
                return BadRequest(exception.Message);
            }
        }
        #endregion

        #region POST

        [HttpPost("rateOrder")]
        public async Task<ActionResult<bool>> RateOrder(RateOrderDto request)
        {
            try
            {
                return Ok(await _userService.RateOrder(request));
            }
            catch (Exception exception)
            {
                Log.Error("Error occured on [POST] RateOrder");
                return BadRequest(exception.Message);
            }
        }

        [HttpPost("reportOrder")]
        public async Task<ActionResult<bool>> ReportOrder(ReportOrderDto request)
        {
            try
            {
                return Ok(await _userService.ReportOrder(request));
            }
            catch (Exception exception)
            {
                Log.Error("Error occured on [POST] RateOrder");
                return BadRequest(exception.Message);
            }
        }


        #endregion

        #region UPDATE

        [HttpPost("updateBankCard")]
        public async Task<ActionResult<bool>> UpdateBankCard(UpdateBankCardDto cardDto)
        {
            try
            {
                return Ok(await _userService.UpdateBankCard(cardDto));
            }
            catch (Exception exception)
            {
                Log.Error("Error occured on [POST] UpdateBankCard");
                return BadRequest(exception.Message);
            }
        }


        #endregion


        #region ADD
        [HttpPost("makeOrder")]
        public async Task<ActionResult<bool>> MakeOrder(MakeOrderDto request)
        {
            try
            {
                return Ok(await _userService.MakeOrder(request));
            }
            catch (Exception exception)
            {
                Log.Error("Error occured on [POST] MakeOrder");
                return BadRequest(exception.Message);
            }
        }

        
        [HttpPost("addBankCard")]
        public async Task<ActionResult<bool>> AddBankCard(AddBankCardDto cardDto)
        {
            try
            {
                return Ok(await _userService.AddBankCard(cardDto));
            }
            catch (Exception exception)
            {
                Log.Error("Error occured on [POST] AddBankCard");
                return BadRequest(exception.Message);
            }
        }


        #endregion


        #region REMOVE

        [HttpDelete("removeBankCard")]
        public async Task<ActionResult<bool>> RemoveBankCard(string cardId)
        {
            try
            {
                return Ok(await _userService.RemoveBankCard(cardId));
            }
            catch (Exception exception)
            {
                Log.Error("Error occured on [DELETE] RemoveBankCard");
                return BadRequest(exception.Message);
            }
        }

        [HttpDelete("RemoveProfile")]
        public async Task<ActionResult<bool>> RemoveProfile(string userId)
        {
            try
            {
                return Ok(await _userService.RemoveProfile(userId));
            }
            catch (Exception exception)
            {
                Log.Error("Error occured on [DELETE] RemoveProfile");
                return BadRequest(exception.Message);
            }

        }

        #endregion 
    }
}
