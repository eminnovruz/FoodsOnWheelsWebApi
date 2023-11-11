using Application.Repositories.Repository;
using Domain.Models.Common;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories;

public class WriteRepository<T> : IWriteRepository<T> where T : BaseEntity
{
    private readonly AppDbContext _context;
    public WriteRepository(AppDbContext context)
    {
        _context = context;
    }

    DbSet<T> Table => _context.Set<T>();

    public async Task<bool> AddAsync(T entity)
    {
        var entry = await Table.AddAsync(entity);
        return entry.State == EntityState.Added;
    }

    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await Table.AddRangeAsync(entities);
    }

    public bool Remove(T entity)
    {
        var entry = Table.Remove(entity);
        return entry.State == EntityState.Deleted;
    }

    public async Task<bool> RemoveAsync(string id)
    {
        var entry = Table.Remove(Table.FirstOrDefault(e => e.Id == id));
        return entry.State == EntityState.Deleted;
    }

    public bool Update(T entity)
    {
        var entry = Table.Update(entity);
        return entry.State == EntityState.Modified;
    }

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
}
