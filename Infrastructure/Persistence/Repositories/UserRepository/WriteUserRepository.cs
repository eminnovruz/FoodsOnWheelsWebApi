using Application.Repositories.UserRepository;
using Domain.Models;
using Persistence.Context;
using Persistence.Repositories.Repository;

namespace Persistence.Repositories.UserRepository;

public class WriteUserRepository : WriteRepository<User>, IWriteUserRepository
{
    public WriteUserRepository(AppDbContext context) : base(context)
    {
    }
}
