using Domain.Models.Common;

namespace Domain.Models;

public class Restaurant : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Rating { get; set; }


}
