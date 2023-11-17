using System.Security.Claims;

namespace Application.Services;

public interface IJWTService
{
    string GenerateSecurityToken(string id, string email);
}