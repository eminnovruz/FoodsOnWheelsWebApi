﻿using Application.Repositories.Repository;
using Domain.Models;

namespace Application.Repositories.CategoryRepository;

public interface IWriteCategoryRepository : IWriteRepository<Category>
{
}
