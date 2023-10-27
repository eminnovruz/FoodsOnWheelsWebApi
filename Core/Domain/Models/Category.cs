using Domain.Models.Common;

namespace Domain.Models;

public class Category : BaseEntity
{
    public string CategoryName { get; set; }
    public string CategoryDescription { get; set; }
    public IEnumerable<Food> Foods { get; set; }
}
