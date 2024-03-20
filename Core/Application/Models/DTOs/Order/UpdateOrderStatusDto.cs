using Domain.Models.Enums;

namespace Application.Models.DTOs.Order;

public class UpdateOrderStatusDto
{
    public string OrderId { get; set; }
    public OrderStatus OrderStatus { get; set; }
}