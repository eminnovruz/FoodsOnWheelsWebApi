using Application.Models.DTOs.AppUser;
using FluentValidation;

namespace WebApi.Validators
{
    public class UpdateAppUserPasswordDtoValidator : AbstractValidator<UpdateAppUserPasswordDto>
    {
        public UpdateAppUserPasswordDtoValidator()
        {
            RuleFor(c => c.NewPassword)
                .NotEmpty()
                .WithMessage("Password is required!")
                .MinimumLength(8)
                .WithMessage("Password must be longer than 8 chars!")
                .MaximumLength(20)
                .WithMessage("Password cannot be longer than 20 chars!");
            
            RuleFor(c => c.OldPassword)
                .NotEmpty()
                .WithMessage("Password is required!")
                .MinimumLength(8)
                .WithMessage("Password must be longer than 8 chars!")
                .MaximumLength(20)
                .WithMessage("Password cannot be longer than 20 chars!");


            RuleFor(c => c.Id)
                .NotEmpty()
                .WithMessage("Id is required!");
        }


     }

}
