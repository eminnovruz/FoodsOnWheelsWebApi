using Application.Repositories.BankCardRepository;
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
    public class WriteBankCardRepository : WriteRepository<BankCard> ,IWriteBankCardRepository
    {
        public WriteBankCardRepository(AppDbContext context) : base(context)
        {

        }
    }
}

