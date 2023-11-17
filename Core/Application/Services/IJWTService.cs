using System.Security.Claims;

namespace Application.Services;

public interface IJWTService
{
    string GenerateSecurityToken(string id, string email, IEnumerable<string> roles, IEnumerable<Claim> userClaims);
}