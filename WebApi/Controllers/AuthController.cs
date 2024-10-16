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


        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthTokenDto>> Login([FromBody] LoginRequest request)
        {
            try
            {
                var token = await _authService.LoginUser(request);
                Log.Information($"{request.Email} LogIn.");
                return Ok(token);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("register")]
        public async Task<ActionResult<AuthTokenDto>> Register([FromBody] AddUserDto request)
        {
            try
            {
                var token = await _authService.RegisterUser(request);
                if (token is not null)
                {
                    Log.Information($"{request.Email} registered.");
                    return Ok(token);
                }
                throw new ArgumentException("Something get wrong!");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("refreshToken")]
        public async Task<ActionResult<AuthTokenDto>> RefreshToken([FromBody]RefreshTokenDto request)
        {
            try
            {
                var token = await _authService.RefreshToken(request);
                return Ok(token);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return BadRequest(ex.Message);
            }
        }

    }
}
