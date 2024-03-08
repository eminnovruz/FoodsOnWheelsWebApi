using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.DTOs.Order
{
    public class AcceptOrderDto
    {
        public string OrderId { get; set; }
        public string CourierId { get; set; }
    }
}
