namespace Application.Models.DTOs.Auth.JWT;

public class RefreshTokenRequest
{
    public string RefreshToken { get; set; } = string.Empty;
}
