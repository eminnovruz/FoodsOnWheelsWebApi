using Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.DTOs.Order
{
    public class UpdateOrderStatusDto
    {
        public string OrderId { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }
}
