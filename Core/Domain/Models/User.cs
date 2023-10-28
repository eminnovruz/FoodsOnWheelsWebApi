using Domain.Models.Common;
namespace Domain.Models;

public class User : BaseEntity
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime BirthDate { get; set; }
    public IEnumerable<Order> Orders { get; set; }
}
