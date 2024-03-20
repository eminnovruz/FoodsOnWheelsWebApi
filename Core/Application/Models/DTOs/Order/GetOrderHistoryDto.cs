namespace Application.Models.DTOs.Order;

public class GetOrderHistoryDto
{
    public string OrderId { get; set; }
    public uint Amount { get; set; }
    public string CourierId { get; set; }
    public string CourierName { get; set; }
    public uint CourierRating { get; set; }
    public string RestaurantId { get; set; }
    public string RestaurantName { get; set; }
    public List<string> FoodIds { get; set; }
}