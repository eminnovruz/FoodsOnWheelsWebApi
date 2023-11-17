﻿using Application.Models;
using Application.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services;

public class JWTService : IJWTService
{
    private readonly JWTConfiguration _config;

    public JWTService(JWTConfiguration config)
    {
        _config = config;
    }

    public string GenerateSecurityToken(string id, string email, IEnumerable<string> roles, IEnumerable<Claim> userClaims)
    {
        var claims = new[]
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, email),
                new Claim("userId", id)
            };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Secret));

        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config.Issuer,
            audience: _config.Audience,
            expires: DateTime.UtcNow.AddMonths(_config.ExpireMonths),
            signingCredentials: signingCredentials,
            claims: claims
            );

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

        return accessToken;
    }
}
