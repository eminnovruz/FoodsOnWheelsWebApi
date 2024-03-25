using Application.Models.DTOs.Courier;
using FluentValidation;

namespace WebApi.Validators;

public class AddCourierDtoValidator : AbstractValidator<AddCourierDto>
{
    public AddCourierDtoValidator()
    {
        RuleFor(c => c.Password)
            .NotEmpty()
            .WithMessage("Password is required!")
            .MinimumLength(8)
            .WithMessage("Password must be longer than 8 chars!")
            .MaximumLength(20)
            .WithMessage("Password cannot be longer than 20 chars!");

        RuleFor(c => c.Email)
            .NotEmpty()
            .WithMessage("Email address is required!")
            .Must(c => c.EndsWith("@gmail.com"))
            .WithMessage("Email address is not accurate!");

        RuleFor(c => c.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number is required!")
            .MaximumLength(10)
            .WithMessage("Phone number is not accurate!");

        RuleFor(c => c.Name)
            .NotEmpty()
            .WithMessage("Name is required!");

        RuleFor(c => c.Surname)
            .NotEmpty()
            .WithMessage("Surname is required!");

    }
}
