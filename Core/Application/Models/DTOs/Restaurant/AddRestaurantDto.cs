using Microsoft.AspNetCore.Http;

namespace Application.Models.DTOs.Restaurant;

public class AddRestaurantDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public List<string> FoodIds { get; set; }
    public IFormFile File { get; set; }
}