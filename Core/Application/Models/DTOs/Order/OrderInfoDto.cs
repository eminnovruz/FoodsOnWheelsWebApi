namespace Application.Models.DTOs.Order;

public class OrderInfoDto
{
    public DateTime OrderDate { get; set; }
    public string RestaurantId { get; set; }
    public List<string> FoodIds { get; set; }
    public bool PayedWithCard { get; set; }
    public string UserId { get; set; }
    public int Rate { get; set; }
}
