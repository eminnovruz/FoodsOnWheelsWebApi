using Application.Repositories.RestaurantRepository;
using Application.Repositories.WorkerRepository;
using Domain.Models;
using Persistence.Context;
using Persistence.Repositories.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.WorkerRepository
{
    public class WriteWorkerRepository : WriteRepository<Worker>, IWriteWorkerRepository
    {
        public WriteWorkerRepository(AppDbContext context) : base(context)
        {
        }
    }
}
