using Application.Repositories.CategoryRepository;
using Application.Repositories.CourierRepository;
using Domain.Models;
using Persistence.Context;

namespace Persistence.Repositories.CategoryRepository;

public class ReadCategoryRepository : ReadRepository<Category>, IReadCategoryRepository
{
    public ReadCategoryRepository(AppDbContext context) : base(context)
    {
    }
}
