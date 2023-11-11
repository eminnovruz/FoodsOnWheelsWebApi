using Application.Models.DTOs.Courier;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<GetProfileInfoDto> ViewCourierProfile(string courierId)
        {
            return await _courierService.GetProfileInfo(courierId);
        }
    }
}
