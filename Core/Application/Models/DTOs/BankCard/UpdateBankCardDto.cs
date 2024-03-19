using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.DTOs.BankCard
{
    public class UpdateBankCardDto
    {
        public string Id { get; set; }
        public string CardNumber { get; set; }
        public string ExpireDate { get; set; }
        public string CardOwnerFullName { get; set; }
        public string CVV { get; set; }
    }
}
