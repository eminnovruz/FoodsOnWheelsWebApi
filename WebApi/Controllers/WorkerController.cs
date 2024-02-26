using Application.Models.DTOs.Category;
using Application.Models.DTOs.Courier;
using Application.Models.DTOs.Food;
using Application.Models.DTOs.Restaurant;
using Application.Models.DTOs.Worker;
using Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;

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

        [HttpPost("addNewRestaurant")]
        public async Task<ActionResult<bool>> AddNewRestaurant([FromForm]AddRestaurantDto request)
        {
            try
            {
                return Ok(await _workerService.AddRestaurant(request));
            }
            catch (Exception exception)
            {
                Log.Error("error occured with [POST] AddNewRestaurant");
                return BadRequest(exception.Message);   
            }
        }

        // Restaurant Funcs
        [HttpPost("addNewFood")]
        public ActionResult<bool> AddNewFood([FromForm] AddFoodRequest request)
        {
            try
            {
                return Ok(_workerService.AddNewFood(request));
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpPost("addNewCategory")]
        public async Task<ActionResult<bool>> AddNewCategory([FromForm] AddCategoryRequest request)
        {
            try
            {
                return Ok(await _workerService.AddCategory(request));
            }
            catch (Exception exception)
            {
                Log.Error("error occured with [POST] AddNewCategory");
                return BadRequest(exception.Message);
            }
        }


        [HttpDelete("removeCategory")]
        public async Task<ActionResult<bool>> RemoveCategory(string categoryId)
        {
            try
            {
                return Ok( _workerService.RemoveCategory(categoryId));
            }
            catch (Exception exception)
            {
                Log.Error("error occured on [DELETE] RemoveCourier");
                return BadRequest(exception.Message);
            }
        }


    }
}
