﻿using Application.Models.DTOs.Category;
using Application.Models.DTOs.Food;
using Application.Models.DTOs.Order;
using Application.Models.DTOs.Restaurant;
using Application.Services.IUserServices;
using Azure.Core;
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

        [HttpPost("inLastDecidesSituation")]
        public async Task<ActionResult<bool>> InLastDecidesSituation(InLastSituationOrderDto orderDto)
        {
            try
            {
                return Ok(await restaurantService.InLastDecidesSituation(orderDto));
            }
            catch (Exception exception)
            {
                Log.Error($"Error occured on [POST] InLastDecidesSituation : {exception.Message}");
                return BadRequest(exception.Message);
            }
        }

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

        [HttpGet("waitingOrders")]
        public ActionResult<IEnumerable<OrderInfoDto>> WaitingOrders(string resturantId)
        {
            try
            {
                return Ok(restaurantService.WaitingOrders(resturantId));
            }
            catch (Exception exception)
            {
                Log.Error($"Error occured on [GET] WaitingOrders : {exception.Message}");
                return BadRequest(exception.Message);
            }
        }

        [HttpPost("updateStatusOrder")]
        public async Task<ActionResult<bool>> UpdateStatusOrder(UpdateOrderStatusDto statusDto)
        {
            try
            {
                return Ok(await restaurantService.UpdateStatusOrder(statusDto));
            }
            catch (Exception exception)
            {
                Log.Error($"Error occured on [POST] UpdateStatusOrder : {exception.Message}");
                return BadRequest(exception.Message);
            }
        }

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
    }
}
