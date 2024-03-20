namespace Application.Models.DTOs.Food;

public class UpdateFoodRequest
{
    public string Id { get; set; }
    public string Name { get; set; }
    public List<string> CategoryIds { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public uint Price { get; set; }
}