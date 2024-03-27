using Application.Models.DTOs.AppUser;
using FluentValidation;

namespace WebApi.Validators
{

    public class AddAppUserDtoValidator : AbstractValidator<AddAppUserDto>
    {
        public AddAppUserDtoValidator()
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

            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage("Name is required!");
        }
    }
}
