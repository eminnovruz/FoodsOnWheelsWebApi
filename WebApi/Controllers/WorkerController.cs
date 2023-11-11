using Application.Models.DTOs.Courier;
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


        }


    }
}
