using Application.Models.DTOs.Restaurant;
using FluentValidation;

namespace WebApi.Validators;

public class AddRestaurantDtoValidator : AbstractValidator<AddRestaurantDto>
{
    public AddRestaurantDtoValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty()
            .WithMessage("Restaurant name is required")
            .MaximumLength(20)
            .WithMessage("Name cannot be longer than 20 char");

        RuleFor(r => r.Description)
            .NotEmpty()
            .WithMessage("Description for restaurant is required")
            .MaximumLength(50)
            .WithMessage("Description cannot be longer than 50 char");

        RuleFor(x => x.FoodIds)
            .Must(foodIds => foodIds != null && foodIds.Any())
            .WithMessage("At least one food ID must be provided.");
    }
}
