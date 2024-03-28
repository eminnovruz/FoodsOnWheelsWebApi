using Application.Models.DTOs.AppUser;

namespace Application.Models.DTOs.Courier;

public class UpdateCourierDto : UpdateAppUserDto
{
    public string Surname { get; set; }
    public DateTime BirthDate { get; set; }
    public string PhoneNumber { get; set; }

}