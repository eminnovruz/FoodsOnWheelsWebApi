namespace Application.Models.DTOs.Order;

public class MakeOrderDto
{
    public DateTime OrderDate { get; set; }
    public string RestaurantId { get; set; }
    public List<string> FoodIds { get; set; }
    public bool PayWithCard { get; set; }
    public string CardNumber { get; set; }
    public string ExpireDate { get; set; }
    public string CardOwnerFullName { get; set; }
    public string UserId { get; set; }
}
