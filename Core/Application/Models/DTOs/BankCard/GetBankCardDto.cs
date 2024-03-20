namespace Application.Models.DTOs.BankCard;

public class GetBankCardDto
{
    public string UserId { get; set; }
    public string CardNumber { get; set; }
    public string ExpireDate { get; set; }
    public string CardOwnerFullName { get; set; }
    public string CVV { get; set; }
}