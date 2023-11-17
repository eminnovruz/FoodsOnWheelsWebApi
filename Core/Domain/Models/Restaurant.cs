using Domain.Models.Common;

namespace Domain.Models;

public class Restaurant : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public uint Rating { get; set; }
    public List<string> FoodIds { get; set; }
    public List<string> CommentIds { get; set; }
    public string ImageUrl { get; set; }
}
