namespace Application.Models.DTOs.Order;

public class ReportOrderDto
{
    public string RestaurantId { get; set; }
    public string OrderId { get; set; }
    public string UserId { get; set; }
    public string Content { get; set; }
    public bool ContactWithMe { get; set; }
    public byte Rate { get; set; }
}
