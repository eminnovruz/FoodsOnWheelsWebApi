namespace Application.Models.DTOs.Courier;

public class GetProfileInfoDto
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime BirthDate { get; set; }
    public List<string> OrderIds { get; set; }
    public int Rating { get; set; }
}
