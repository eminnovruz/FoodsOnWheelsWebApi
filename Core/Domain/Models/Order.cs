using Domain.Models.Common;
using Domain.Models.Enums;

namespace Domain.Models;

public class Order : BaseEntity
{
    public string OrderRatingId { get; set; }
    public string RestaurantId { get; set; }
    public string CourierId { get; set; }
    public string UserId { get; set; }
    public List<string> OrderedFoodIds { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime OrderFinishTime { get; set; } = default;
    public uint Amount { get; set; }
    public bool PayedWithCard { get; set; }
    public bool IsActivated { get; set; } 
    public OrderStatus OrderStatus { get; set; }
}
