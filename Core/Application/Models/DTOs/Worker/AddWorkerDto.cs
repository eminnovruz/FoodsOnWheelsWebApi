namespace Application.Models.DTOs.Worker;

public class AddWorkerDto
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
    public DateTime BirthDate { get; set; }
}