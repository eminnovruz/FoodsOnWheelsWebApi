namespace Application.Models.DTOs.Worker;

public class AddCourierDto
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string PasswordHash { get; set; }
    public DateTime BirthDate { get; set; }
}
