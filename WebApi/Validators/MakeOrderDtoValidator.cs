using Application.Models.DTOs.Order;
using FluentValidation;

namespace WebApi.Validators;

public class MakeOrderDtoValidator : AbstractValidator<MakeOrderDto>
{
    public MakeOrderDtoValidator()
    {
        RuleFor(x => x.FoodIds)
            .Must(foodIds => foodIds != null && foodIds.Any())
            .WithMessage("At least one food ID must be provided.");

        RuleFor(x => x.RestaurantId)
            .NotEmpty()
            .WithMessage("Restaurant id must be provided.");
    }
}
