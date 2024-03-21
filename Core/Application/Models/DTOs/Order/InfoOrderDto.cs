using Domain.Models.Enums;

namespace Application.Models.DTOs.Order;

public class InfoOrderDto
{
    public string Id { get; set; }
    public DateTime OrderDate { get; set; }
    public string RestaurantId { get; set; }
    public List<string> FoodIds { get; set; }
    public bool PayedWithCard { get; set; }
    public string UserId { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public uint Amount { get; set; }
    public byte Rate { get; set; }
}