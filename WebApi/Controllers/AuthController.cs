using Application.Models.DTOs;
using Application.Models.DTOs.Auth;
using Application.Models.DTOs.Auth.JWT;
using Application.Services;
using Domain.Models;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IJWTService _jwtService;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthController(UserManager<User> userManager, IJWTService jwtService, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        private async Task<AuthTokenDto> GenerateToken(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var claims = await _userManager.GetClaimsAsync(user);

            var accessToken = _jwtService.GenerateSecurityToken(user.Id, user.Email, roles, claims);

            var refreshToken = Guid.NewGuid().ToString("N").ToLower();
            user.RefreshToken = refreshToken;

            await _userManager.UpdateAsync(user);

            return new AuthTokenDto()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterRequestDto request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser is not null)
                return Conflict("User already exists");

            var user = new User
            {
                Name = request.Name,
                Surname = request.Surname,
                Email = request.Email,
                RefreshToken = Guid.NewGuid().ToString("N").ToLower(),
                BirthDate = request.BirthDate,
                Id = Guid.NewGuid().ToString(),
                OrderIds = new List<string>(),
                PhoneNumber = request.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            if(request.IsCourier)
            {
                await _userManager.AddToRoleAsync(user, "Courier");
            }
            else
            {
                await _userManager.AddToRoleAsync(user, "User");
            } 

            return Ok("Registration successful");
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return BadRequest("Invalid email or password");
            }
            if (await _userManager.IsEmailConfirmedAsync(user))
            {
                var canSignIn = await _signInManager.PasswordSignInAsync(user, request.Password, false,  false);

                if (!canSignIn.Succeeded)
                    return BadRequest("Invalid email or password");

                var token = await GenerateToken(user);
                return Ok(token);
            }
            return Unauthorized("Email not confirmed");
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(e => e.RefreshToken == request.RefreshToken);

            if (user is null)
                return Unauthorized("Invalid refresh token");

            var token = await GenerateToken(user);
            return Ok(token);
        }
    }
}
