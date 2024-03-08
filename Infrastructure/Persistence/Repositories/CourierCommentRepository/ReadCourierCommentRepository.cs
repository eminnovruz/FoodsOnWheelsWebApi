using Application.Repositories.CourierCommentRepository;
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
    public class ReadCourierCommentRepository : ReadRepository<CourierComment>, IReadCourierCommentRepository
    {
        public ReadCourierCommentRepository(AppDbContext context) : base(context)
        {

        }

    }
}
