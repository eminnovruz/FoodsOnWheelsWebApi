using Domain.Models.Common;

namespace Domain.Models;

public class Order : BaseEntity
{
    public virtual Courier Courier { get; set; }
    public virtual User User { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime OrderFinishTime { get; set; }
    public virtual IEnumerable<Food> OrderedFoods { get; set; }
    public uint Amount { get; set; }
    public virtual Restaurant Restoraunt { get; set; }
}
