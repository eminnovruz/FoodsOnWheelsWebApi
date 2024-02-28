using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.DTOs.Food
{
    public class AddFoodToRestaurantDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public uint Price { get; set; }
        public string RestaurantId { get; set; }
        public List<string> CategoryIds { get; set; }
        public IFormFile File { get; set; }
    }
}
