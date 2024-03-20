namespace Application.Models.DTOs.Courier;

public class UpdateCourierDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public DateTime BirthDate { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}