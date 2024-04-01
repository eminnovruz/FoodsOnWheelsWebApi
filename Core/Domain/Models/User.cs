using Domain.Models.Common;

namespace Domain.Models;

public class User : AppUser
{
    public string Surname { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime BirthDate { get; set; }
    public string SelectBankCardId { get; set; }
    public List<string> BankCardsId { get; set; }
    public List<string> OrderIds { get; set; }
}
