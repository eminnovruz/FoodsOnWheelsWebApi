using Application.Repositories.Repository;
using Domain.Models;

namespace Application.Repositories.UserRepository;

public interface IWriteUserRepository : IWriteRepository<User>
{
}
