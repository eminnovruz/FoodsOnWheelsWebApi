using Application.Repositories.RestaurantCommentRepository;
using Persistence.Context;
using Persistence.Repositories.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.RestaurantCommentRepository
{
    public class ReadRestaurantCommentRepository : ReadRepository<RestaurantComment> , IReadRestaurantCommentRepository
    {
        public ReadRestaurantCommentRepository(AppDbContext context) : base(context)
        {

        }
    }
}