using Application.Models.DTOs.Courier;
using Application.Models.DTOs.Restaurant;
using Application.Models.DTOs.Worker;
using Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkerController : ControllerBase
    {
        private readonly IWorkerService _workerService;
        public WorkerController(IWorkerService service)
        {
            _workerService = service;
        }

        // Courier Funcs

        [HttpGet("getAllCouriers")]
        public async Task<ActionResult<IEnumerable<SummaryCourierDto>>> GetAllCouriers()
        {
            try
            {
                var result = await _workerService.GetAllCouriers();
                return Ok(result);
            }
            catch (Exception exception)
            {
                Log.Error("error occured on [GET] GetAllCouriers");
                return BadRequest(exception.Message);
            }
        }

        [HttpPost("addNewCourier")]
        public async Task<ActionResult<bool>> AddNewCourier(AddCourierDto request)
        {
            try
            {
                return Ok(await _workerService.AddCourier(request));
            }
            catch (Exception exception)
            {
                Log.Error("error occured on [POST] AddNewCourier");
                return BadRequest(exception.Message);
            }
        }

        [HttpDelete("removeCourier")]
        public async Task<ActionResult<bool>> RemoveCourier(string courierId)
        {
            try
            {
                return Ok(await _workerService.RemoveCourier(courierId));
            }
            catch (Exception exception)
            {
                Log.Error("error occured on [DELETE] RemoveCourier");
                return BadRequest(exception.Message);
            }
        }

        // Restaurant Funcs

    }
}
