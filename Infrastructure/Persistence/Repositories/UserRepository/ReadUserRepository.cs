using Application.Repositories.UserRepository;
using Domain.Models;
using Persistence.Context;

namespace Persistence.Repositories.UserRepository;

public class ReadUserRepository : ReadRepository<User>, IReadUserRepository
{
    public ReadUserRepository(AppDbContext context) : base(context)
    {
    }
}
