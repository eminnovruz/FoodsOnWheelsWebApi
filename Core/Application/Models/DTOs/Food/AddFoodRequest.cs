using Microsoft.AspNetCore.Http;

namespace Application.Models.DTOs.Food;

public class AddFoodRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public uint Price { get; set; }
    public string RestaurantId { get; set; }
    public List<string> CategoryIds { get; set; }
    public IFormFile File { get; set; }
}