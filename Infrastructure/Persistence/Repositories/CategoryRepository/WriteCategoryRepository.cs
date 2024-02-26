using Application.Repositories.CategoryRepository;
using Application.Repositories.CourierRepository;
using Domain.Models;
using Persistence.Context;
using Persistence.Repositories.Repository;

namespace Persistence.Repositories.CategoryRepository;

public class WriteCategoryRepository : WriteRepository<Category>, IWriteCategoryRepository
{
    public WriteCategoryRepository(AppDbContext context) : base(context)
    {
    }
}
