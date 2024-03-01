using Domain.Models.Common;

namespace Domain.Models;

public class Order : BaseEntity
{
    public string CourierId { get; set; }
    public string UserId { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime OrderFinishTime { get; set; } = default;
    public List<string> OrderedFoodIds { get; set; }
    public uint Amount { get; set; }
    public bool PayedWithCard { get; set; }
    public string RestaurantId { get; set; }
    public bool IsActivated { get; set; } 
    public string OrderRatingId { get; set; }
}
