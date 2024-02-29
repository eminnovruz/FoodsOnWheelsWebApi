﻿using Application.Models.DTOs.Category;
using Application.Models.DTOs.Food;
using Application.Models.DTOs.Order;
using Application.Models.DTOs.Restaurant;
using Application.Models.DTOs.User;

namespace Application.Services.IUserServices;

public interface IUserService
{
    IEnumerable<RestaurantInfoDto> GetAllRestaurants();

    IEnumerable<CategoryInfoDto> GetAllFoodCategories();

    IEnumerable<FoodInfoDto> GetFoodsByCategory(string categoryId);

    Task<IEnumerable<FoodInfoDto>> GetFoodsByRestaurant(string restaurantId);

    Task<GetUserProfileInfoDto> GetProfileInfo(string userId);

    Task<bool> RateOrder(RateOrderDto request);

    Task<bool> ReportOrder(ReportOrderDto request);

    Task<bool> MakeOrder(MakeOrderDto request);
}