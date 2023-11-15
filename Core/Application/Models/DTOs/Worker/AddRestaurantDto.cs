namespace Application.Models.DTOs.Worker;

public class AddRestaurantDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public List<string> FoodIds { get; set; }
}
