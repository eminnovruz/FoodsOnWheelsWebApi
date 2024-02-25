namespace Application.Models.DTOs.Food;

public class AddFoodRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public uint Price { get; set; }
    public List<string> CategoryIds { get; set; }
    public string ImageUrl { get; set; }
}
