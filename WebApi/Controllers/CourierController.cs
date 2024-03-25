using Application.Models.DTOs.Comment;
using Application.Models.DTOs.Courier;
using Application.Models.DTOs.Order;
using Application.Services.IUserServices;
using Microsoft.AspNetCore.Mvc;
using Serilog;

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

        [HttpGet("getProfileInfo")]
        public async Task<ActionResult<GetProfileInfoDto>> GetProfileInfo(string courierId)
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


        [HttpGet("GetOrderHistory")]
        public async Task<ActionResult<IEnumerable<InfoOrderDto>>> GetOrderHistory(string courierId)
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
                return Ok(GetNewOrder());
            }
            catch (Exception exception)
            {
                Log.Error("Error occured on [GET] GetNewOrder");
                return BadRequest(exception.Message);
            }
        }


        [HttpGet("getActiveOrderInfo")]
        public async Task<ActionResult<InfoOrderDto>> GetActiveOrderInfo(string OrderId)
        {
            try
            {
                return Ok(await GetActiveOrderInfo(OrderId));
            }
            catch (Exception exception)
            {
                Log.Error("Error occured on [POST] GetActiveOrderInfo");
                return BadRequest(exception.Message);
            }
        }


        [HttpGet("getAllComments")]
        public async Task<ActionResult<IEnumerable<GetCommentDto>>> GetAllComments(string CourierId)
        {
            try
            {
                return Ok(await GetAllComments(CourierId));
            }
            catch (Exception exception)
            {
                Log.Error("Error occured on [POST] GetAllComments");
                return BadRequest(exception.Message);
            }
        }
     

        [HttpGet("getPastOrderInfoById")]
        public async Task<ActionResult<InfoOrderDto>> GetPastOrderInfoById(string PastOrderId)
        {
            try
            {
                return Ok(await GetPastOrderInfoById(PastOrderId));
            }
            catch (Exception exception)
            {
                Log.Error("Error occured on [POST] GetPastOrderInfoById");
                return BadRequest(exception.Message);
            }
        }


        [HttpPost("acceptOrder")]
        public async Task<ActionResult<bool>> AcceptOrder(AcceptOrderFromCourierDto request)
        {
            try
            {
                return Ok(await AcceptOrder(request));
            }
            catch (Exception exception)
            {
                Log.Error("Error occured on [POST] AcceptOrder");
                return BadRequest(exception.Message);
            }
        }


        [HttpPost("rejectOrder")]
        public async Task<ActionResult<bool>> RejectOrder(RejectOrderDto orderDto)
        {
            try
            {
                return Ok(await RejectOrder(orderDto));
            }
            catch (Exception exception)
            {
                Log.Error("Error occured on [POST] RejectOrder");
                return BadRequest(exception.Message);
            }
        }


    }
}
