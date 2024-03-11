namespace Application.Models.DTOs.Comment;

public class GetCommentDto
{
    public string CourierId { get; set; }
    public string Content { get; set; }
    public DateTime CommentDate { get; set; }
    public string OrderId { get; set; }
    public uint Rate { get; set; }
}