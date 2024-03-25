using Application.Models.DTOs.Auth;
using Application.Models.DTOs.Category;
using Application.Models.DTOs.Courier;
using Application.Models.DTOs.Food;
using Application.Models.DTOs.Order;
using Application.Models.DTOs.Restaurant;
using Application.Models.DTOs.User;
using Application.Models.DTOs.Worker;
using Application.Services.IUserServices;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WorkerController : ControllerBase
{
    private readonly IWorkerService _workerService;
    public WorkerController(IWorkerService service)
    {
        _workerService = service;
    }


    #region Restaurant
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


    [HttpPut("updateRestaurant/{restaurantId}")]
    public async Task<ActionResult<bool>> UpdateRestaurant([FromForm]UpdateRestaurantDto request)
    {
        try
        {
            return Ok(await _workerService.UptadeRestaurant(request));
        }
        catch (Exception exception)
        {
            Log.Error("Error occurred on [PUT] UpdateRestaurant");
            return BadRequest(exception.Message);
        }
    }


    [HttpDelete("removeRestaurant/{restaurantId}")]
    public async Task<ActionResult<bool>> RemoveRestaurant(string restaurantId)
    {
        try
        {
            var result = await _workerService.RemoveRestaurant(restaurantId);
            return Ok(result);
        }
        catch (Exception exception)
        {
            Log.Error("Error occurred on [DELETE] RemoveRestaurant");
            return BadRequest(exception.Message);
        }
    }


    [HttpGet("getRestaurantById/{restaurantId}")]
    public async Task<ActionResult<RestaurantInfoDto>> GetRestaurantById(string restaurantId)
    {
        try
        {
            var restaurant = await _workerService.GetRestaurantById(restaurantId);

            if (restaurant == null)
                return NotFound(); 

            return Ok(restaurant);
        }
        catch (Exception exception)
        {
            Log.Error("Error occurred on [GET] GetRestaurantById");
            return BadRequest(exception.Message);
        }
    }


    [HttpGet("getAllRestaurants")]
    public ActionResult<IEnumerable<RestaurantInfoDto>> GetAllRestaurants()
    {
        try
        {
            var restaurants = _workerService.GetAllRestaurants();

            if (restaurants == null || !restaurants.Any())
                return NotFound(); 

            return Ok(restaurants);
        }
        catch (Exception exception)
        {
            Log.Error("Error occurred on [GET] GetAllRestaurants");
            return BadRequest(exception.Message);
        }
    }
    #endregion


    #region Category
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


    [HttpPut("updateCategory/{categoryId}")]
    public async Task<ActionResult<bool>> UpdateCategory([FromBody] UpdateCategoryRequest request)
    {
        try
        {
            var result = await _workerService.UpdateCategory(request);
            return Ok(result);
        }
        catch (Exception exception)
        {
            Log.Error("Error occurred on [PUT] UpdateCategory");
            return BadRequest(exception.Message);
        }
    }


    [HttpDelete("removeCategory")]
    public ActionResult<bool> RemoveCategory(string categoryId)
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


    [HttpGet("getCategoryById/{categoryId}")]
    public async Task<ActionResult<CategoryInfoDto>> GetCategoryById(string categoryId)
    {
        try
        {
            var category = await _workerService.GetCategoryById(categoryId);

            if (category == null)
                return NotFound(); 

            return Ok(category);
        }
        catch (Exception exception)
        {
            Log.Error("Error occurred on [GET] GetCategoryById");
            return BadRequest(exception.Message);
        }
    }


    [HttpGet("getAllCategories")]
    public ActionResult<IEnumerable<CategoryInfoDto>> GetAllCategories()
    {
        try
        {
            var categories =  _workerService.SeeAllCategories();

            if (categories == null || !categories.Any())
            {
                return NotFound(); 
            }

            return Ok(categories);
        }
        catch (Exception exception)
        {
            Log.Error("Error occurred on [GET] GetAllCategories");
            return BadRequest(exception.Message);
        }
    }
    #endregion


    #region Courier
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


    [HttpPut("updateCourier")]
    public async Task<ActionResult<bool>> UpdateCourier(UpdateCourierDto request)
    {
        try
        {
            return Ok(await _workerService.UpdateCourier(request));
        }
        catch (Exception exception)
        {
            Log.Error("Error occurred on [PUT] UpdateCourier");
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


    [HttpGet("getCourierById/{courierId}")]
    public async Task<ActionResult<SummaryCourierDto>> GetCourierById(string courierId)
    {
        try
        {
            var courier = await _workerService.GetCourierById(courierId);

            if (courier == null)
                return NotFound(); 

            return Ok(courier);
        }
        catch (Exception exception)
        {
            Log.Error("Error occurred on [GET] GetCourierById");
            return BadRequest(exception.Message);
        }
    }


    [HttpGet("getAllCouriers")]
    public ActionResult<IEnumerable<SummaryCourierDto>> GetAllCouriers()
    {
        try
        {
            var result =  _workerService.GetAllCouriers();
            return Ok(result);
        }
        catch (Exception exception)
        {
            Log.Error("error occured on [GET] GetAllCouriers");
            return BadRequest(exception.Message);
        }
    }
    #endregion


    #region Worker
    [HttpPost("addWorker")]
    public async Task<ActionResult<bool>> AddWorker(AddWorkerDto request)
    {
        try
        {
            var result = await _workerService.AddWorker(request);
            return Ok(result);
        }
        catch (Exception exception)
        {
            Log.Error("Error occurred on [POST] AddWorker");
            return BadRequest(exception.Message);
        }
    }


    [HttpPut("updateWorker/{workerId}")]
    public async Task<ActionResult<bool>> UpdateWorker([FromBody] UpdateWorkerDto request)
    {
        try
        {
            var result = await _workerService.UpdateWorker(request);
            return Ok(result);
        }
        catch (Exception exception)
        {
            Log.Error("Error occurred on [PUT] UpdateWorker");
            return BadRequest(exception.Message);
        }
    }


    [HttpDelete("removeWorker/{workerId}")]
    public async Task<ActionResult<bool>> RemoveWorker(string workerId)
    {
        try
        {
            var result = await _workerService.RemoveWorker(workerId);
            return Ok(result);
        }
        catch (Exception exception)
        {
            Log.Error("Error occurred on [DELETE] RemoveWorker");
            return BadRequest(exception.Message);
        }
    }


    [HttpGet("getWorkerById/{workerId}")]
    public async Task<ActionResult<GetWorkerDto>> GetWorkerById(string workerId)
    {
        try
        {
            var worker = await _workerService.GetWorkerById(workerId);

            if (worker == null)
                return NotFound(); 

            return Ok(worker);
        }
        catch (Exception exception)
        {
            Log.Error("Error occurred on [GET] GetWorkerById");
            return BadRequest(exception.Message);
        }
    }


    [HttpGet("getAllWorkers")]
    public ActionResult<IEnumerable<GetWorkerDto>> GetAllWorkers()
    {
        try
        {
            var workers = _workerService.GetAllWorkers();
            return Ok(workers);
        }
        catch (Exception exception)
        {
            Log.Error("Error occurred on [GET] GetAllWorkers");
            return BadRequest(exception.Message);
        }
    }
    #endregion


    #region Food
    [HttpPost("addNewFood")]
    public ActionResult<bool> AddNewFood([FromForm] AddFoodRequest request)
    {
        try
        {
            return Ok(_workerService.AddNewFood(request));
        }
        catch (Exception exception)
        {
            Log.Error("error occured with [POST] AddNewRestaurant");
            return BadRequest(exception.Message);   
        }
    }


    [HttpPut("updateFood/{foodId}")]
    public async Task<ActionResult<bool>> UpdateFood([FromForm] UpdateFoodRequest request)
    {
        try
        {
            var result = await _workerService.UpdateFood(request);
            return Ok(result);
        }
        catch (Exception exception)
        {
            Log.Error("Error occurred on [PUT] UpdateFood");
            return BadRequest(exception.Message);
        }
    }


    [HttpDelete("removeFood/{foodId}")]
    public async Task<ActionResult<bool>> RemoveFood(string foodId)
    {
        try
        {
            var result = await _workerService.RemoveFood(foodId);
            return Ok(result);
        }
        catch (Exception exception)
        {
            Log.Error("Error occurred on [DELETE] RemoveFood");
            return BadRequest(exception.Message);
        }
    }


    [HttpGet("getFoodById/{foodId}")]
    public async Task<ActionResult<FoodInfoDto>> GetFoodById(string foodId)
    {
        try
        {
            var food = await _workerService.GetFoodById(foodId);

            if (food == null)
                return NotFound();

            return Ok(food);
        }
        catch (Exception exception)
        {
            Log.Error("Error occurred on [GET] GetFoodById");
            return BadRequest(exception.Message);
        }
    }


    [HttpGet("getAllFoods")]
    public ActionResult<IEnumerable<FoodInfoDto>> GetAllFoods()
    {
        try
        {
            var foods = _workerService.SeeAllFoods();

            if (foods == null || !foods.Any())
                return NotFound(); 

            return Ok(foods);
        }
        catch (Exception exception)
        {
            Log.Error("Error occurred on [GET] GetAllFoods");
            return BadRequest(exception.Message);
        }
    }
    #endregion


    #region User
    [HttpPost("addNewUser")]
    public async Task<ActionResult<bool>> AddNewUser([FromBody] UserRegisterRequest request)
    {
        try
        {
            var result = await _workerService.AddUser(request);
            return Ok(result);
        }
        catch (Exception exception)
        {
            Log.Error("Error occurred on [POST] AddNewUser");
            return BadRequest(exception.Message);
        }
    }


    [HttpPut("updateUser/{userId}")]
    public async Task<ActionResult<bool>> UpdateUser([FromBody] UpdateUserDto request)
    {
        try
        {
            var result = await _workerService.UpdateUser(request);
            return Ok(result);
        }
        catch (Exception exception)
        {
            Log.Error("Error occurred on [PUT] UpdateUser");
            return BadRequest(exception.Message);
        }
    }


    [HttpDelete("removeUser/{userId}")]
    public async Task<ActionResult<bool>> RemoveUser(string userId)
    {
        try
        {
            var result = await _workerService.RemoveUser(userId);
            return Ok(result);
        }
        catch (Exception exception)
        {
            Log.Error("Error occurred on [DELETE] RemoveUser");
            return BadRequest(exception.Message);
        }
    }


    [HttpGet("getUserById/{userId}")]
    public async Task<ActionResult<GetUserProfileInfoDto>> GetUserById(string userId)
    {
        try
        {
            var user = await _workerService.GetUserById(userId);

            if (user == null)
                return NotFound();

            return Ok(user);
        }
        catch (Exception exception)
        {
            Log.Error("Error occurred on [GET] GetUserById");
            return BadRequest(exception.Message);
        }
    }


    [HttpGet("getAllUsers")]
    public ActionResult<IEnumerable<GetUserProfileInfoDto>> GetAllUsers()
    {
        try
        {
            var users = _workerService.GetAllUsers();

            if (users == null || !users.Any())
                return NotFound();

            return Ok(users);
        }
        catch (Exception exception)
        {
            Log.Error("Error occurred on [GET] GetAllUsers");
            return BadRequest(exception.Message);
        }
    }
    #endregion

}