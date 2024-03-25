using Domain.Models.Common;

namespace Domain.Models;

public class CourierComment : BaseEntity
{
    public string CourierId { get; set; }
    public string Content { get; set; }
    public DateTime CommentDate { get; set; }
    public string OrderId { get; set; }
    public uint Rate { get; set; }




}
