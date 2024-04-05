namespace Domain.Models;

public class Worker : AppUser
{
    public string Surname { get; set; }
    public DateTime BirthDate { get; set; }
    public string PhoneNumber { get; set; }
}
