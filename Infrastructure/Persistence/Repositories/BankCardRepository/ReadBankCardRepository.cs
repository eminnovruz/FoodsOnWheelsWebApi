using Application.Repositories.BankCardRepository;
using Application.Repositories.Repository;
using Domain.Models;
using Persistence.Context;
using Persistence.Repositories.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.BankCardRepository
{
    public class ReadBankCardRepository : ReadRepository<BankCard> , IReadBankCardRepository
    {
        public ReadBankCardRepository(AppDbContext context) : base(context)
        {
        }
    }
}
