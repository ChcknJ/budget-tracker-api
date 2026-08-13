using BudgetTracker.DTO;
using FluentValidation;

namespace BudgetTracker.Validators
{
    public class ExpenseFilterRequestValidator : AbstractValidator<ExpenseFilterRequest>
    {
        public ExpenseFilterRequestValidator()
        {
            RuleFor(x => x.ToDate).GreaterThanOrEqualTo(x => x.FromDate).When(x => x.FromDate.HasValue && x.ToDate.HasValue).WithMessage("End date must be greater than or equal to start date.");

            RuleFor(x => x.MinimumAmount).LessThanOrEqualTo(x => x.MaximumAmount).When(x => x.MinimumAmount.HasValue && x.MaximumAmount.HasValue).WithMessage("Minimum amount must be less than or equal to maximum amount.");
        }
    }
}
