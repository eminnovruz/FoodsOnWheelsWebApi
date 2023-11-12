using Domain.Models.Common;

namespace Domain.Models;

public class Order : BaseEntity
{
    public string CourierId { get; set; }
    public string UserId { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime OrderFinishTime { get; set; }
    public List<string> OrderedFoodIds { get; set; }
    public uint Amount { get; set; }
    public string RestorauntId { get; set; }
    public bool IsActivated { get; set; } 
    public string OrderRatingId { get; set; }
}
