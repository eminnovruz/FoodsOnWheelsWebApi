using Application.Models.DTOs.AppUser;

namespace Application.Models.DTOs.Worker;

public class UpdateWorkerDto : UpdateAppUserDto
{
    public string Id { get; set; }
    public string Surname { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime BirthDate { get; set; }
}