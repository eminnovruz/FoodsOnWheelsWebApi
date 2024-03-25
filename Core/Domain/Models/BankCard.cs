using Domain.Models.Common;


namespace Domain.Models
{
    public class BankCard : BaseEntity
    {
        public string UserId { get; set; }
        public string CardNumber { get; set; }
        public string ExpireDate { get; set; }
        public string CardOwnerFullName { get; set; }
        public string CVV { get; set; }
    }
}
