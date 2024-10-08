﻿using Application.Models.DTOs.Auth;
using System.Security.Claims;

namespace Application.Services.IAuthServices;

public interface IJWTService
{
    AuthTokenDto GenerateSecurityToken(string id, string email, string role);
}