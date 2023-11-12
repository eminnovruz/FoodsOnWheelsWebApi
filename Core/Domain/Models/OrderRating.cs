using Domain.Models.Common;

namespace Domain.Models;

public class OrderRating : BaseEntity
{
    public byte Rate { get; set; }
    public string Content { get; set; }
}
