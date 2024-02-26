using Application.Repositories.UserRepository;
using Domain.Models;
using Persistence.Context;
using Persistence.Repositories.Repository;

namespace Persistence.Repositories.UserRepository;

public class ReadUserRepository : ReadRepository<User>, IReadUserRepository
{
    public ReadUserRepository(AppDbContext context) : base(context)
    {
    }
}
