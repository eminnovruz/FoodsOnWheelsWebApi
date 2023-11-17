using Application.Models.DTOs;
using Application.Models.DTOs.Auth;
using Application.Models.DTOs.Auth.JWT;
using Application.Services;
using Domain.Models;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
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
        //[HttpPost("login")]
        //public async Task<ActionResult> Login([FromBody] UserLoginRequest request)
        //{

        //}
    }
}
