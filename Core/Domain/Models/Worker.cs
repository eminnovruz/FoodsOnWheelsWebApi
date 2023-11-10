using Domain.Models.Common;

namespace Domain.Models;

public class Worker : BaseEntity
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public DateTime BirthDate { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string PasswordHash { get; set; }
    public uint Rating { get; set; }
}
