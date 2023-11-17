namespace Application.Models.DTOs.Auth;

public class RegisterRequestDto
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set;}
    public string PhoneNumber { get; set; }
    public DateTime BirthDate { get; set; }
    public bool IsCourier { get; set; }
}
