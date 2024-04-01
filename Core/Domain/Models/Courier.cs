using Domain.Models.Common;

namespace Domain.Models;


public class Courier : BaseEntity
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime BirthDate { get; set; }
    public byte[] PassHash { get; set; }
    public byte[] PassSalt { get; set; }
    public string ActiveOrderId {  get; set; }
    public List<string> OrderIds { get; set; }
    public List<string> CourierCommentIds { get; set; }
    public float Rating { get; set; }
}
