using BudgetTracker.DTO;
using FluentValidation;

namespace BudgetTracker.Validators
{
    public class BudgetRequestValidator : AbstractValidator<BudgetRequest>
    {
        public BudgetRequestValidator()
        {
            RuleFor(x => x.Month).NotEmpty().WithMessage("Month is required.");
            RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Amount must be greater than 0.");
        }
    }
}
