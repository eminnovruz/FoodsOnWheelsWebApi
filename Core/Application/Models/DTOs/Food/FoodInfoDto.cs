namespace Application.Models.DTOs.Food;

public class FoodInfoDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public uint Price { get; set; }
    public List<string> CategoryIds { get; set; }
}
