using Application.Repositories.OrderRatingRepository;
using Domain.Models;
using Persistence.Context;
using Persistence.Repositories.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.OrderRatingRepository
{
    public class ReadOrderRatingRepository : ReadRepository<OrderRating> , IReadOrderRatingRepository
    {
        public ReadOrderRatingRepository(AppDbContext context) : base(context)
        {
        }
    }
}