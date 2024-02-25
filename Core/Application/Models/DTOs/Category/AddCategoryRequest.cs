namespace Application.Models.DTOs.Category;

public class AddCategoryRequest
{
    public string CategoryName { get; set; }
    public List<string> FoodIds { get; set; }
}
