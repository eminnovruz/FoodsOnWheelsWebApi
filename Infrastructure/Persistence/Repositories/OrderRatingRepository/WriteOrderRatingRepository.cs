using Application.Repositories.OrderRatingRepository;
using Domain.Models;
using Persistence.Context;
using Persistence.Repositories.Repository;

namespace Persistence.Repositories.OrderRatingRepository;

public class WriteOrderRatingRepository : WriteRepository<OrderRating> , IWriteOrderRatingRepository
{
    public WriteOrderRatingRepository(AppDbContext context) : base(context)
    {
    }
}

