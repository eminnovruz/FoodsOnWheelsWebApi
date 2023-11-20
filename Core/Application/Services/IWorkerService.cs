﻿using Application.Models.DTOs.Courier;
using Application.Models.DTOs.Restaurant;
using Application.Models.DTOs.Worker;
using Domain.Models;

namespace Application.Services;

public interface IWorkerService
{
    Task<bool> AddRestaurant(AddRestaurantDto dto);
    Task<bool> AddCourier(AddCourierDto dto);
    Task<IEnumerable<RestaurantInfoDto>> GetAllRestaurants();
    Task<bool> RemoveRestaurant(string restaurantId);
    Task<bool> RemoveCourier(string courierId);
    Task<IEnumerable<Food>> SeeAllFoods();
    Task<IEnumerable<SummaryCourierDto>> GetAllCouriers();
}
