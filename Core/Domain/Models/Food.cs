using Domain.Models.Common;

namespace Domain.Models;

public class Food : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public uint Price { get; set; }
    public List<string> CategoryIds { get; set; }
    public string RestaurantId { get; set; }
    public string ImageUrl { get; set; }

}
