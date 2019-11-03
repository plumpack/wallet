using FluentValidation;
using PlumPack.Wallet.Web.Features.Account.Models;

namespace PlumPack.Wallet.Web.Features.Account.Validators
{
    public class AddFundsInputValidator : AbstractValidator<AddFundsInputModel>
    {
        public AddFundsInputValidator()
        {
            // You can only add between 25 dollars and 1000 dollars at a time.
            // TODO: Make configurable
            RuleFor(x => x.Amount).GreaterThanOrEqualTo(25).LessThanOrEqualTo(1000);
        }   
    }
}