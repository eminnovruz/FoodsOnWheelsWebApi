using Domain.Models.Common;


namespace Domain.Models;

public class Category : BaseEntity
{
    public string CategoryName { get; set; }
    public List<string> FoodIds { get; set; }
}