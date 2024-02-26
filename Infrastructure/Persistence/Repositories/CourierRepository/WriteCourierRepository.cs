using Application.Repositories.CourierRepository;
using Application.Repositories.Repository;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Repositories.Repository;
using System.Security.Policy;

namespace Persistence.Repositories.CourierRepository;

public class WriteCourierRepository : WriteRepository<Courier> , IWriteCourierRepository
{
    public WriteCourierRepository(AppDbContext context)
        : base(context)
    {
        
    }
}
