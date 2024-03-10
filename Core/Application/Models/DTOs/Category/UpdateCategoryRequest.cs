using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.DTOs.Category
{
    public class UpdateCategoryRequest
    {
        public string Id { get; set; }
        public string CategoryName { get; set; }
        public List<string> FoodIds { get; set; }
    }
}
