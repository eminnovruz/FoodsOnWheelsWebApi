using Domain.Models.Common;

namespace Domain.Models;

public class Restaurant : AppUser
{
    public string Description { get; set; }
    public float Rating { get; set; }
    public List<string> FoodIds { get; set; }
    public List<string> CommentIds { get; set; }
    public List<string> OrderIds { get; set; }
    public string ImageUrl { get; set; }
}
