using Domain.Models.Common;

namespace Domain.Models;

public class Food : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public uint Price { get; set; }
    public virtual IEnumerable<Category> Categories { get; set; }
}
