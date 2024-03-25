using Domain.Models.Common;


public class RestaurantComment : BaseEntity
{
    public string RestaurantId { get; set; }
    public string OrderId { get; set; }
    public string Content { get; set; }
    public uint Rating { get; set; }
    public bool ContactWithMe { get; set; }
    public DateTime CommentDate { get; set; }
}