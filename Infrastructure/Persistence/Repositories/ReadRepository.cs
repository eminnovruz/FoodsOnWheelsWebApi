using Application.Repositories.Repository;
using Domain.Models.Common;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System.Linq.Expressions;

namespace Persistence.Repositories;

public class ReadRepository<T> : IReadRepository<T> where T : BaseEntity
{
    private readonly AppDbContext _context;
    public ReadRepository(AppDbContext context)
    {
        _context = context;
    }

    DbSet<T> Table => _context.Set<T>();

    public IEnumerable<T?> GetWhere(Expression<Func<T, bool>> expression) => Table.Where(expression);
    public async Task<T?> GetAsync(Expression<Func<T, bool>> expression) => await Table.FirstOrDefaultAsync(expression);

    public async Task<T?> GetAsync(string id) => await Table.FirstOrDefaultAsync(e => e.Id == id);

    public IEnumerable<T?> GetAll(bool tracking = true)
    {
        if (tracking)
            return Table.ToList();

        return Table.AsNoTracking().ToList();
    }


}
