using System.Security.Claims;

namespace Application.Services.IAuthServices;

public interface IJWTService
{
    string GenerateSecurityToken(string id, string email, string role);
}