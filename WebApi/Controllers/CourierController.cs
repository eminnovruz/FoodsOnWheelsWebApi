using Application.Models.DTOs.Courier;
using Application.Models.DTOs.Order;
using Application.Services.IUserServices;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Reflection;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourierController : ControllerBase
    {
        private readonly ICourierService _courierService; 

        public CourierController(ICourierService courierService)
        {
            _courierService = courierService;
        }

        [HttpGet("viewCourierProfile")]
        public async Task<ActionResult<GetProfileInfoDto>> ViewCourierProfile(string courierId)
        {
            try
            {
                var user = await _courierService.GetProfileInfo(courierId);
            
                if (user is null)
                    return BadRequest($"Cannot find user - {courierId}. Maybe deleted or missing");
            
                return Ok(user);
            }
            catch (Exception exception)
            {
                Log.Error("Error occured on [GET] ViewCourierProfile");
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("viewOrderHistory")]
        public async Task<ActionResult<IEnumerable<OrderInfoDto>>> ViewOrderHistory(string courierId)
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

    }
}
