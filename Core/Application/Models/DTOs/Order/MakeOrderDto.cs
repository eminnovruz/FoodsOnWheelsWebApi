namespace Application.Models.DTOs.Order;

public class MakeOrderDto
{
    public DateTime OrderDate { get; set; }
    public string RestaurantId { get; set; }
    public List<string> FoodIds { get; set; }
    public bool PayWithCard { get; set; }
    public string BankCardId { get; set; } = string.Empty;
    public string UserId { get; set; }
}
