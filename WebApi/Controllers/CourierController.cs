﻿using Application.Models.DTOs.Comment;
using Application.Models.DTOs.Courier;
using Application.Models.DTOs.Order;
using Application.Models.DTOs.User;
using Application.Services.IUserServices;
using Infrastructure.Services.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Courier")]
    [ApiController]
    public class CourierController : ControllerBase
    {
        private readonly ICourierService _courierService;

        public CourierController(ICourierService courierService)
        {
            _courierService = courierService;
        }



        #region GET

        [HttpGet("GetOrderHistory")]
        public async Task<ActionResult<IEnumerable<InfoOrderDto>>> GetOrderHistory([FromBody] string courierId)
        {
            try
            {
                var pastOrders = await _courierService.GetOrderHistory(courierId);

                return Ok(pastOrders);
            }
            catch (Exception exception)
            {
                Log.Error("Error occured on [GET] ViewOrderHistory");
                return BadRequest(exception.Message);
            }
        }


        [HttpGet("getNewOrder")]
        public ActionResult<List<InfoOrderDto>> GetNewOrder()
        {
            try
            {
                return Ok(_courierService.GetNewOrder());
            }
            catch (Exception exception)
            {
                Log.Error("Error occured on [GET] GetNewOrder");
                return BadRequest(exception.Message);
            }
        }


        [HttpGet("getActiveOrderInfo")]
        public async Task<ActionResult<InfoOrderDto>> GetActiveOrderInfo([FromBody] string OrderId)
        {
            try
            {
                return Ok(await _courierService.GetActiveOrderInfo(OrderId));
            }
            catch (Exception exception)
            {
                Log.Error("Error occured on [POST] GetActiveOrderInfo");
                return BadRequest(exception.Message);
            }
        }


        [HttpGet("getAllComments")]
        public async Task<ActionResult<IEnumerable<GetCommentDto>>> GetAllComments([FromBody] string CourierId)
        {
            try
            {
                return Ok(await _courierService.GetAllComments(CourierId));
            }
            catch (Exception exception)
            {
                Log.Error("Error occured on [POST] GetAllComments");
                return BadRequest(exception.Message);
            }
        }


        [HttpGet("getPastOrderInfoById")]
        public async Task<ActionResult<InfoOrderDto>> GetPastOrderInfoById([FromBody] string PastOrderId)
        {
            try
            {
                return Ok(await _courierService.GetPastOrderInfoById(PastOrderId));
            }
            catch (Exception exception)
            {
                Log.Error("Error occured on [POST] GetPastOrderInfoById");
                return BadRequest(exception.Message);
            }
        }


        [HttpGet("getProfileInfo")]
        public async Task<ActionResult<GetProfileInfoDto>> GetProfileInfo([FromBody] string courierId)
        {
            try
            {
                var user = await _courierService.GetProfileInfo(courierId);

                if (user is null)
                    return BadRequest($"Cannot find user - {courierId}. Maybe deleted or missing.");

                return Ok(user);
            }
            catch (Exception exception)
            {
                Log.Error("Error occured on [GET] ViewCourierProfile");
                return BadRequest(exception.Message);
            }
        }

        #endregion


        #region UPDATE

        [HttpPut("updateProfile")]
        public async Task<ActionResult<bool>> UpdateProfile([FromBody] UpdateCourierDto dto)
        {
            try
            {
                return Ok(await _courierService.UpdateProfile(dto));
            }
            catch (Exception exception)
            {
                Log.Error("Error occured on [PUT] UpdateProfile");
                return BadRequest(exception.Message);
            }
        }


        [HttpPut("updateProfilePasssword")]
        public async Task<ActionResult<bool>> UpdateProfilePasssword([FromBody] UpdateCourierPasswordDto dto)
        {
            try
            {
                return Ok(await _courierService.UpdateProfilePasssword(dto));
            }
            catch (Exception exception)
            {
                Log.Error("Error occured on [PUT] UpdateProfilePasssword");
                return BadRequest(exception.Message);
            }
        }

        #endregion


        #region REMOVE

        [HttpDelete("removeProfile")]
        public async Task<ActionResult<bool>> RemoveProfile([FromBody] string courierId)
        {
            try
            {
                return Ok(await _courierService.RemoveProfile(courierId));
            }
            catch (Exception exception)
            {
                Log.Error("Error occured on [POST] RemoveProfile");
                return BadRequest(exception.Message);
            }
        }


        [HttpPost("rejectOrder")]
        public async Task<ActionResult<bool>> RejectOrder([FromBody] RejectOrderDto orderDto)
        {
            try
            {
                return Ok(await _courierService.RejectOrder(orderDto));
            }
            catch (Exception exception)
            {
                Log.Error("Error occured on [POST] RejectOrder");
                return BadRequest(exception.Message);
            }
        }

        #endregion


        #region ADD

        [HttpPost("acceptOrder")]
        public async Task<ActionResult<bool>> AcceptOrder([FromBody] AcceptOrderFromCourierDto request)
        {
            try
            {
                return Ok(await _courierService.AcceptOrder(request));
            }
            catch (Exception exception)
            {
                Log.Error("Error occured on [POST] AcceptOrder");
                return BadRequest(exception.Message);
            }
        }


        #endregion

    }
}
