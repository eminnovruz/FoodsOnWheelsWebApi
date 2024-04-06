using Application.Models.DTOs.Auth;
using Application.Models.DTOs.User;
using Application.Repositories;
using Application.Services.IAuthServices;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;

        public AuthController(IAuthService authService, IUnitOfWork unitOfWork)
        {
            _authService = authService;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var token = await _authService.LoginUser(request);
                return Ok(token);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }


        //[HttpPost("loginUser")]
        //public ActionResult<AuthTokenDto> LoginUser(LoginRequest request)
        //{
        //    try
        //    {
        //        var token
        //    }

        //    catch (Exception ex)
        //    {
        //        Log.Error(ex.Message);
        //        return BadRequest(ex.Message);
        //    }

        //}

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AddUserDto request)
        {
            try
            {
                if (await _authService.RegisterUser(request))
                {
                    Log.Information($"{request.Email} registered.");
                    return Ok("Successfully Registered!");
                }
                throw new Exception("Something get wrong!");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
