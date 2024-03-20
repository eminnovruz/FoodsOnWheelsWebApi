namespace Application.Models.DTOs.Order;

public class RateOrderDto
{
    public string OrderId { get; set; }
    public byte Rate { get; set; }  
    public string Content { get; set; }
    public uint CourierRate { get; set; }
    public string CourierContent { get; set; }

}