﻿using Application.Repositories.CourierRepository;
using Application.Repositories.RestaurantRepository;
using Domain.Models;
using Persistence.Context;
using Persistence.Repositories.Repository;

namespace Persistence.Repositories.RestaurantRepository;

public class WriteRestaurantRepository : WriteRepository<Restaurant>, IWriteRestaurantRepository
{
    public WriteRestaurantRepository(AppDbContext context) : base(context)
    {
    }
}
