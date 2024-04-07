﻿using Application.Repositories.Repository;
using Domain.Models;

namespace Application.Repositories.UserRepository;

public interface IReadUserRepository : IReadRepository<User>
{
}
