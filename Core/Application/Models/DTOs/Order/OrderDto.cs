namespace Application.Models.DTOs.Order;

public class OrderDto
{
    public string OrderId { get; set; }
    public string CourierId { get; set; }
    public string UserId { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime OrderFinishTime { get; set; }
    public List<string> OrderedFoodIds { get; set; }
    public uint Amount { get; set; }
    public string RestorauntId { get; set; }
}
