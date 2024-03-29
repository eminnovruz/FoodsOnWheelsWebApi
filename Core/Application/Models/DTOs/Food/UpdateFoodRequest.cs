using Microsoft.AspNetCore.Http;

namespace Application.Models.DTOs.Food;

public class UpdateFoodRequest
{
    public string Id { get; set; }
    public string Name { get; set; }
    public List<string> CategoryIds { get; set; }
    public string Description { get; set; }
    public IFormFile File { get; set; } = null;
    public uint Price { get; set; }
}