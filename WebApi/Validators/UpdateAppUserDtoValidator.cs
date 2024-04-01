using Application.Models.DTOs.AppUser;
using FluentValidation;

namespace WebApi.Validators
{
    public class UpdateAppUserDtoValidator : AbstractValidator<UpdateAppUserDto>
    {
        public UpdateAppUserDtoValidator()
        {
            RuleFor(c => c.Email)
                .NotEmpty()
                .WithMessage("Email address is required!")
                .Must(c => c.EndsWith("@gmail.com"))
                .WithMessage("Email address is not accurate!");

            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage("Name is required!");

            RuleFor(c => c.Id)
                .NotEmpty()
                 .WithMessage("Id is required!");
        }
    }
}
