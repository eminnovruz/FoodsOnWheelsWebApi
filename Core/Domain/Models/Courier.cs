using Domain.Models.Common;

namespace Domain.Models;

public class Courier : BaseEntity
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public IEnumerable<Order> Orders { get; set; }
    public uint Rating { get; set; }
}
