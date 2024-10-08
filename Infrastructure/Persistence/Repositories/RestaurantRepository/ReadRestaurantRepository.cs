﻿using Application.Repositories.CourierRepository;
using Application.Repositories.RestaurantRepository;
using Domain.Models;
using Persistence.Context;
using Persistence.Repositories.Repository;

namespace Persistence.Repositories.RestaurantRepository;

public class ReadRestaurantRepository : ReadRepository<Restaurant>, IReadRestaurantRepository
{
    public ReadRestaurantRepository(AppDbContext context) : base(context)
    {
    }
}
