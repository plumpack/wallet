using FluentValidation;
using PlumPack.Wallet.Web.Features.AddFunds.Models;

namespace PlumPack.Wallet.Web.Features.AddFunds.Validators
{
    public class SpecifyAmountInputValidator : AbstractValidator<SpecifyAmountInputModel>
    {
        public SpecifyAmountInputValidator()
        {
            // You can only add between 25 dollars and 1000 dollars at a time.
            // TODO: Make configurable
            RuleFor(x => x.Amount).GreaterThanOrEqualTo(25).LessThanOrEqualTo(1000);
        }   
    }
}