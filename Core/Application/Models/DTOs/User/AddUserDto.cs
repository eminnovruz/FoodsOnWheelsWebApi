using Application.Models.DTOs.AppUser;

namespace Application.Models.DTOs.User;

public class AddUserDto : AddAppUserDto
{
    public string Surname { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime BirthDate { get; set; }
}