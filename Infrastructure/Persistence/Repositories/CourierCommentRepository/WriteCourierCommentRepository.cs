using Application.Repositories.CourierCommentRepository;
using Application.Repositories.RestaurantCommentRepository;
using Domain.Models;
using Persistence.Context;
using Persistence.Repositories.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories.CourierCommentRepository
{
    public class WriteCourierCommentRepository : WriteRepository<CourierComment>, IWriteCourierCommentRepository
    {
        public WriteCourierCommentRepository(AppDbContext context) : base(context)
        {

        }
    }
}
