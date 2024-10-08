﻿using Application.Models.DTOs.AppUser;
using Application.Models.DTOs.BankCard;
using Application.Models.DTOs.Category;
using Application.Models.DTOs.Food;
using Application.Models.DTOs.Order;
using Application.Models.DTOs.Restaurant;
using Application.Models.DTOs.User;
using Application.Repositories;
using Application.Services.IAuthServices;
using Application.Services.IUserServices;
using Domain.Models;
using FluentValidation;

namespace Infrastructure.Services.UserServices;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPassHashService _hashService;
    private readonly IValidator<UpdateAppUserPasswordDto> _updateAppUserPasswordValidator;
    private readonly IValidator<UpdateAppUserDto> _updateAppUserValidator;
    private readonly IValidator<AddBankCardDto> _addBankCardDtoValidator;

    public UserService(IUnitOfWork unitOfWork, IPassHashService hashService, IValidator<UpdateAppUserPasswordDto> updateAppUserPasswordValidator, IValidator<UpdateAppUserDto> updateAppUserValidator, IValidator<AddBankCardDto> addBankCardDtoValidator)
    {
        _unitOfWork = unitOfWork;
        _hashService = hashService;
        _updateAppUserPasswordValidator = updateAppUserPasswordValidator;
        _updateAppUserValidator = updateAppUserValidator;
        _addBankCardDtoValidator = addBankCardDtoValidator;
    }





    #region Profile
    public async Task<GetUserProfileInfoDto> GetProfileInfo(string userId)
    {
        var user = await _unitOfWork.ReadUserRepository.GetAsync(userId);

        if (user is null)
            throw new ArgumentNullException("User is not Found");

        return new GetUserProfileInfoDto
        {
            Name = user.Name,
            Surname = user.Surname,
            BirthDate = user.BirthDate,
            Email = user.Email,
            OrderIds = user.OrderIds,
            PhoneNumber = user.PhoneNumber,
        };
    }


    public async Task<bool> RemoveProfile(string userId)
    {
        var user = await _unitOfWork.ReadUserRepository.GetAsync(userId);
        if (user is null)
            throw new ArgumentNullException("User not found");

        var bankCard = _unitOfWork.ReadBankCardRepository.GetWhere(x => x.UserId == userId);
        foreach (var item in bankCard)
        {
            if (item is not null)
                await _unitOfWork.WriteBankCardRepository.RemoveAsync(item.Id);
        }
        await _unitOfWork.WriteUserRepository.RemoveAsync(userId);


        await _unitOfWork.WriteBankCardRepository.SaveChangesAsync();
        await _unitOfWork.WriteUserRepository.SaveChangesAsync();

        return true;

    }


    public async Task<bool> UpdateProfile(UpdateUserDto dto)
    {
        var isValid = _updateAppUserValidator.Validate(dto);

        if (isValid.IsValid)
        {
            var existingUser = await _unitOfWork.ReadUserRepository.GetAsync(dto.Id);

            if (existingUser is null)
                throw new ArgumentException("User not found");


            existingUser.Name = dto.Name;
            existingUser.Surname = dto.Surname;
            existingUser.BirthDate = dto.BirthDate;
            existingUser.Email = dto.Email;
            existingUser.PhoneNumber = dto.PhoneNumber;

            var result = await _unitOfWork.WriteUserRepository.UpdateAsync(existingUser.Id);
            await _unitOfWork.WriteUserRepository.SaveChangesAsync();

            return result;
        }
        else
            throw new ArgumentException("No Valid");
    }

    public async Task<bool> UpdateProfilePasssword(UpdateUserPasswordDto dto)
    {
        var isValid = _updateAppUserPasswordValidator.Validate(dto);

        if (isValid.IsValid)
        {
            var existingUser = await _unitOfWork.ReadUserRepository.GetAsync(dto.Id);

            if (existingUser is null)
                throw new ArgumentException("User not found");

            if (!_hashService.ConfirmPasswordHash(dto.OldPassword, existingUser.PassHash, existingUser.PassSalt))
                throw new ArgumentException("Wrong password!");
            _hashService.Create(dto.NewPassword, out byte[] passHash, out byte[] passSalt);

            existingUser.PassSalt = passSalt;
            existingUser.PassHash = passHash;

            var result = await _unitOfWork.WriteUserRepository.UpdateAsync(existingUser.Id);
            await _unitOfWork.WriteUserRepository.SaveChangesAsync();

            return result;
        }
        else
            throw new ArgumentException("No Valid");
    }
    #endregion

    #region Get 

    public IEnumerable<RestaurantInfoDto> GetAllRestaurants()
    {
        var restaurants = _unitOfWork.ReadRestaurantRepository.GetAll().ToList();

        if (restaurants.Count == 0)
            throw new ArgumentNullException("Restaurant not found");

        var restaurantDtos = new List<RestaurantInfoDto>();
        foreach (var item in restaurants)
        {
            if (item is not null)
                restaurantDtos.Add(new RestaurantInfoDto
                {
                    Id = item.Id,
                    Description = item.Description,
                    FoodIds = item.FoodIds,
                    Name = item.Name,
                    Rating = item.Rating,
                    ImageUrl = item.ImageUrl
                });
        }
        return restaurantDtos;
    }

    public IEnumerable<FoodInfoDto> GetFoodsByCategory(string categoryId)
    {
        var allFoods = _unitOfWork.ReadFoodRepository.GetAll().ToList();
        if (allFoods.Count == 0)
            throw new ArgumentNullException("Food not found");

        var foods = new List<Food>();
        foreach (var item in allFoods)
            if (item is not null && item.CategoryIds.Contains(categoryId))
                foods.Add(item);

        var foodDtos = new List<FoodInfoDto>();
        foreach (var food in foods)
        {
            foodDtos.Add(new FoodInfoDto
            {
                CategoryIds = food.CategoryIds,
                Description = food.Description,
                Id = food.Id,
                Name = food.Name,
                Price = food.Price,
                ImageUrl = food.ImageUrl
            });
        }

        return foodDtos;
    }

    public async Task<IEnumerable<FoodInfoDto>> GetFoodsByRestaurant(string restaurantId)
    {
        var restaurant = await _unitOfWork.ReadRestaurantRepository.GetAsync(restaurantId);

        if (restaurant is null)
            throw new ArgumentNullException("Restaurant not found");

        List<FoodInfoDto> dtos = new List<FoodInfoDto>();
        foreach (var item in restaurant.FoodIds)
        {
            var food = await _unitOfWork.ReadFoodRepository.GetAsync(item); ;
            if (food is not null)
            {
                var dto = new FoodInfoDto()
                {
                    CategoryIds = food.CategoryIds,
                    Description = food.Description,
                    Id = food.Id,
                    Name = food.Name,
                    Price = food.Price,
                    ImageUrl = food.ImageUrl
                };
                dtos.Add(dto);
            }
        }

        return dtos;
    }

    public IEnumerable<CategoryInfoDto> GetAllFoodCategories()
    {
        var categories = _unitOfWork.ReadCategoryRepository.GetAll().ToList();

        if (categories.Count == 0)
            throw new ArgumentNullException("Category not found");

        List<CategoryInfoDto> dtos = new List<CategoryInfoDto>();
        foreach (var item in categories)
        {
            if (item is not null)
            {
                var dto = new CategoryInfoDto()
                {
                    CategoryName = item.CategoryName,
                    FoodIds = item.FoodIds,
                    Id = item.Id
                };
                dtos.Add(dto);
            }
        }

        return dtos;
    }

    #endregion

    #region Order

    public async Task<bool> MakeOrder(MakeOrderDto request)
    {

        var user = await _unitOfWork.ReadUserRepository.GetAsync(request.UserId);
        if (user is null)
            throw new ArgumentNullException("User not found");
        var restaurant = await _unitOfWork.ReadRestaurantRepository.GetAsync(request.RestaurantId);
        if (restaurant is null)
            throw new ArgumentNullException("Restaurant not found");

        var newOrder = new Order()
        {
            Id = Guid.NewGuid().ToString(),
            Amount = CalculateOrderAmountAsync(request.FoodIds),
            CourierId = "",
            IsActivated = true,
            OrderDate = DateTime.Now,
            OrderedFoodIds = request.FoodIds,
            UserId = request.UserId,
            OrderRatingId = "",
            RestaurantId = request.RestaurantId,
            OrderStatus = 0,
            OrderFinishTime = default,
        };



        if (request.PayWithCard && user.SelectBankCardId != "")
        {
            newOrder.PayedWithCard = request.PayWithCard;
            newOrder.BankCardId = user.SelectBankCardId;
        }

        user.OrderIds.Add(newOrder.Id);
        restaurant.OrderIds.Add(newOrder.Id);

        await _unitOfWork.WriteRestaurantRepository.UpdateAsync(restaurant.Id);
        await _unitOfWork.WriteRestaurantRepository.SaveChangesAsync();

        await _unitOfWork.WriteUserRepository.UpdateAsync(user.Id);
        await _unitOfWork.WriteUserRepository.SaveChangesAsync();

        var result = await _unitOfWork.WriteOrderRepository.AddAsync(newOrder);
        await _unitOfWork.WriteOrderRepository.SaveChangesAsync();

        return result;

    }


    public async Task<bool> RateOrder(RateOrderDto request)
    {
        var order = await _unitOfWork.ReadOrderRepository.GetAsync(request.OrderId);
        if (order is null)
            throw new ArgumentNullException("Order not found");

        var courier = await _unitOfWork.ReadCourierRepository.GetAsync(order.CourierId);
        if (courier is null)
            throw new ArgumentNullException("Courier not found");

        var orderRating = new OrderRating
        {
            Id = Guid.NewGuid().ToString(),
            Content = request.Content,
            Rate = request.Rate,
        };

        var courierComment = new CourierComment
        {
            Id = Guid.NewGuid().ToString(),
            Rate = request.CourierRate,
            CourierId = order.CourierId,
            CommentDate = DateTime.Now,
            Content = request.CourierContent,
            OrderId = order.Id,
        };

        order.OrderRatingId = orderRating.Id;
        order.OrderFinishTime = DateTime.Now;
        order.IsActivated = false;

        courier.ActiveOrderId = string.Empty;
        courier.CourierCommentIds.Add(courierComment.Id);
        courier.OrderIds.Add(order.Id);

        await _unitOfWork.WriteCourierCommentRepository.AddAsync(courierComment);
        await _unitOfWork.WriteCourierCommentRepository.SaveChangesAsync();

        await _unitOfWork.WriteCourierRepository.UpdateAsync(courier.Id);
        await _unitOfWork.WriteCourierRepository.SaveChangesAsync();

        await _unitOfWork.WriteOrderRatingRepository.AddAsync(orderRating);
        await _unitOfWork.WriteOrderRatingRepository.SaveChangesAsync();

        bool result = await _unitOfWork.WriteOrderRepository.UpdateAsync(order.Id);
        await _unitOfWork.WriteOrderRepository.SaveChangesAsync();

        return result;
    }


    public async Task<bool> ReportOrder(ReportOrderDto request)
    {
        var restaurant = await _unitOfWork.ReadRestaurantRepository.GetAsync(request.RestaurantId);

        if (restaurant is null)
            throw new ArgumentNullException("Restaurant not found");

        var comment = new RestaurantComment
        {
            Id = Guid.NewGuid().ToString(),
            CommentDate = DateTime.Now,
            ContactWithMe = request.ContactWithMe,
            Content = request.Content,
            OrderId = request.OrderId,
            Rating = request.Rate,
            RestaurantId = request.RestaurantId
        };

        restaurant.CommentIds.Add(comment.Id);

        await _unitOfWork.WriteRestaurantCommentRepository.AddAsync(comment);
        await _unitOfWork.WriteRestaurantCommentRepository.SaveChangesAsync();
        await _unitOfWork.WriteRestaurantRepository.UpdateAsync(restaurant.Id);
        await _unitOfWork.WriteRestaurantRepository.SaveChangesAsync();

        return true;
    }

    #endregion

    #region BankCard

    public async Task<bool> AddBankCard(AddBankCardDto cardDto)
    {
        var isValid = _addBankCardDtoValidator.Validate(cardDto);
        if (isValid.IsValid)
        {

            var testCard = await _unitOfWork.ReadBankCardRepository.GetAsync(cardDto.CardNumber);
            if (testCard is not null)
                throw new ArgumentException("This card is unavailable, you must change it");

            var user = await _unitOfWork.ReadUserRepository.GetAsync(cardDto.UserId);
            if (user is null)
                throw new ArgumentException("User not found");

            var newCard = new BankCard
            {
                Id = Guid.NewGuid().ToString(),
                UserId = cardDto.UserId,
                CardNumber = cardDto.CardNumber,
                CVV = cardDto.CVV,
                ExpireDate = cardDto.ExpireDate,
                CardOwnerFullName = cardDto.CardOwnerFullName,
            };

            if (user.BankCardsId.Count == 0)
                user.SelectBankCardId = newCard.Id;

            user.BankCardsId.Add(newCard.Id);

            await _unitOfWork.WriteUserRepository.UpdateAsync(user.Id);
            await _unitOfWork.WriteUserRepository.SaveChangesAsync();

            var result = await _unitOfWork.WriteBankCardRepository.AddAsync(newCard);
            await _unitOfWork.WriteBankCardRepository.SaveChangesAsync();

            return result;
        }
        throw new ArgumentException("No valid");
    }


    public async Task<bool> RemoveBankCard(string cardId)
    {
        var card = await _unitOfWork.ReadBankCardRepository.GetAsync(cardId);
        if (card is null)
            throw new ArgumentNullException("Bank Card not found");

        var user = await _unitOfWork.ReadUserRepository.GetAsync(card.UserId);
        if (user is null)
            throw new ArgumentException("User not found");

        user.BankCardsId.Remove(cardId);

        await _unitOfWork.WriteUserRepository.UpdateAsync(user.Id);
        await _unitOfWork.WriteUserRepository.SaveChangesAsync();

        var result = await _unitOfWork.WriteBankCardRepository.RemoveAsync(card.Id);
        await _unitOfWork.WriteBankCardRepository.SaveChangesAsync();

        return result;
    }

    public async Task<bool> UpdateBankCard(UpdateBankCardDto cardDto)
    {
        var card = await _unitOfWork.ReadBankCardRepository.GetAsync(cardDto.Id);
        if (card is null)
            throw new ArgumentNullException("Bank Card not found");

        card.CVV = cardDto.CVV;
        card.ExpireDate = cardDto.ExpireDate;
        card.CardNumber = cardDto.CardNumber;
        card.CardOwnerFullName = cardDto.CardOwnerFullName;

        var result = await _unitOfWork.WriteBankCardRepository.UpdateAsync(card.Id);
        await _unitOfWork.WriteBankCardRepository.SaveChangesAsync();

        return result;
    }

    public async Task<GetBankCardDto> GetUserBankCard(string cardId)
    {
        var card = await _unitOfWork.ReadBankCardRepository.GetAsync(cardId);
        if (card is null)
            throw new ArgumentNullException("Bank Card not found");

        var cardDto = new GetBankCardDto
        {
            UserId = card.UserId,
            CardNumber = card.CardNumber,
            CVV = card.CVV,
            ExpireDate = card.ExpireDate,
            CardOwnerFullName = card.CardOwnerFullName,
        };
        return cardDto;
    }

    public IEnumerable<GetBankCardDto> GetAllUserBankCard(string userId)
    {
        var cards = _unitOfWork.ReadBankCardRepository.GetWhere(x => x.UserId == userId).ToList();
        if (cards.Count == 0)
            throw new ArgumentNullException("Bank Card not found");

        var cardDtos = new List<GetBankCardDto>();
        foreach (var item in cards)
        {
            if (item is not null)
            {
                cardDtos.Add(new GetBankCardDto
                {
                    UserId = item.UserId,
                    CardNumber = item.CardNumber,
                    CVV = item.CVV,
                    ExpireDate = item.ExpireDate,
                    CardOwnerFullName = item.CardOwnerFullName,
                });
            }
        }

        return cardDtos;
    }

    public async Task<bool> UpdateSelectBankCard(UpdateSelectBankCardDto cardDto)
    {
        var user = await _unitOfWork.ReadUserRepository.GetAsync(cardDto.UserId);
        if (user is null)
            throw new ArgumentException("User not found");

        var card = await _unitOfWork.ReadBankCardRepository.GetAsync(cardDto.BankCardId);
        if (card is null)
            throw new ArgumentNullException("Bank Card not found");

        user.SelectBankCardId = cardDto.BankCardId;


        var result = await _unitOfWork.WriteUserRepository.UpdateAsync(user.Id);
        await _unitOfWork.WriteUserRepository.SaveChangesAsync();

        return result;
    }
    #endregion

    public float CalculateOrderAmountAsync(List<string> foodIds)
    {
        var foods = _unitOfWork.ReadFoodRepository.GetAll().ToList();
        if (foods.Count == 0)
            throw new ArgumentNullException("Food not found");

        float amount = 0;
        foreach (var item in foods)
            if (item is not null && foodIds.Contains(item.Id))
                amount += item.Price;

        return amount;
    }
}