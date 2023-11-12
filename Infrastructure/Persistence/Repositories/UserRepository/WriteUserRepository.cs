using Application.Repositories.UserRepository;
using Domain.Models;
using Persistence.Context;

namespace Persistence.Repositories.UserRepository;

public class WriteUserRepository : WriteRepository<User>, IWriteUserRepository
{
    public WriteUserRepository(AppDbContext context) : base(context)
    {
    }
}
