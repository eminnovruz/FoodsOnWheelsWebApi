using Application.Models.DTOs.Courier;
using Application.Models.DTOs.Worker;
using Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("getAllCouriers")]
        public Task<IEnumerable<SummaryCourierDto>> GetAllCouriers()
        {
            return _workerService.GetAllCouriers();
        }

        [HttpPost("addNewCourier")]
        public ActionResult<bool> AddNewCourier(AddCourierDto request)
        {
            return Ok(_workerService.AddCourier(request));
        }

        [HttpPost("removeCourier")]
        public ActionResult<bool> RemoveCourier(string courierId)
        {
            return Ok(_workerService.RemoveCourier(courierId));
        }
    }
}
