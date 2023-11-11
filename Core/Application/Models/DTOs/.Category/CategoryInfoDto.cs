namespace Application.Models.DTOs.Category;

public class CategoryInfoDto
{
    public string Id { get; set; }
    public string CategoryName { get; set; }
    public List<string> FoodIds { get; set; }
}
