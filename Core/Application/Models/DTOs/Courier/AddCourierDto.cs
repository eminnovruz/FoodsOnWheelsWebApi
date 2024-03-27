using Application.Models.DTOs.AppUser;

namespace Application.Models.DTOs.Courier;

public class AddCourierDto : AddAppUserDto
{
    public string Surname { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime BirthDate { get; set; }
}