namespace Application.Models.DTOs.Courier;

public class SummaryCourierDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime BirthDate { get; set; }
    public int Rating { get; set; }
    public int OrderSize { get; set; }
}