using Application.Models.DTOs.AppUser;
using Application.Models.DTOs.BankCard;
using FluentValidation;

namespace WebApi.Validators
{
    public class AddBankCardDtoValidator : AbstractValidator<AddBankCardDto>
    {
        public AddBankCardDtoValidator()
        {
            RuleFor(x=>x.CardNumber)
                .NotEmpty()
                .Length(16)
                .WithMessage("Card Number is required");

            RuleFor(x => x.CVV)
                .NotEmpty()
                .Length(3)
                .WithMessage("CVV is required");
            RuleFor(x => x.CardOwnerFullName)
                .NotEmpty()
                .WithMessage("Full Name is required");
            RuleFor(x=> x.ExpireDate)
                .Length(5)
                .NotEmpty()
                .WithMessage("ExpireDate is required");
        }
    }
}
