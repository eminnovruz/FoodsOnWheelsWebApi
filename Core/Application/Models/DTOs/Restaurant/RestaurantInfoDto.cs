namespace Application.Models.DTOs.Restaurant;

public class RestaurantInfoDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public float Rating { get; set; }
    public string ImageUrl { get; set; }
    public List<string> FoodIds { get; set; }
}