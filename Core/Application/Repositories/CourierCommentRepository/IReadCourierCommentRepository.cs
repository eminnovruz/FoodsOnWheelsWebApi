using Application.Repositories.Repository;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories.CourierCommentRepository
{
    public interface IReadCourierCommentRepository : IReadRepository<CourierComment>
    {
    }
}
