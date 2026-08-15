using BudgetTracker.DTO;
using FluentValidation;

namespace BudgetTracker.Validators
{
    public class ExpenseQueryRequestValidator : AbstractValidator<ExpenseQueryRequest>
    {
        public ExpenseQueryRequestValidator()
        {
            RuleFor(x => x.ToDate).GreaterThanOrEqualTo(x => x.FromDate).When(x => x.FromDate.HasValue && x.ToDate.HasValue).WithMessage("End date must be greater than or equal to start date.");

            RuleFor(x => x.MinimumAmount).LessThanOrEqualTo(x => x.MaximumAmount).When(x => x.MinimumAmount.HasValue && x.MaximumAmount.HasValue).WithMessage("Minimum amount must be less than or equal to maximum amount.");

            RuleFor(x => x.SortBy).NotEmpty().WithMessage("Sort by is required.").Must(x => x == "date" || x == "amount").WithMessage("Sort by must be Date or Amount");

            RuleFor(x => x.SortOrder).NotEmpty().WithMessage("Sorting Order by is required.").Must(x => x == "ascending" || x == "descending").WithMessage("Sorting Order only accepts Ascending or Descending");

            RuleFor(x => x.PageSize).GreaterThanOrEqualTo(1).LessThanOrEqualTo(20).WithMessage("Page size must be greater than or equal to 1 and less than or equal to 20");

            RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1).WithMessage("Page number must be greater than or equal to 1.");
        }
    }
}
