using Application.Models.DTOs.Courier;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Serilog;
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

                if (user == null)
                {
                    return BadRequest($"Cannot find user - {courierId}. Maybe deleted or missing");
                }

                return user;
            }
            catch (Exception exception)
            {
                Log.Error("Error occured on [GET] ViewCourierProfile");
                return BadRequest(exception.Message);
            }
        }
    }
}
