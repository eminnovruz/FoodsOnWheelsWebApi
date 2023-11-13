namespace Application.Models.DTOs.User;

public class GetUserProfileInfoDto
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }   
    public string PhoneNumber { get; set; }
    public DateTime BirthDate { get; set; }
    public List<string> OrderIds { get; set; }
}
