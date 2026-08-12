using BudgetTracker.DTO;
using FluentValidation;

namespace BudgetTracker.Validators
{
    public class SubscriptionRequestValidator : AbstractValidator<SubscriptionRequest>
    {
        public SubscriptionRequestValidator()
        {
            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Category is required");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required").MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");
            RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Amount must be greater than 0.");
            RuleFor(x => x.StartDate).NotEmpty().WithMessage("Start date is required.");
            RuleFor(x => x.EndDate).GreaterThanOrEqualTo(x => x.StartDate).When(x => x.EndDate.HasValue).WithMessage("End date must be after Start Date");
            RuleFor(x => x.BillingCycle).NotEmpty().WithMessage("Billing Cycle is required.").Must(x => x == "Weekly" || x == "Monthly" || x == "Yearly").WithMessage("Billing cycle must be Weekly, Monthly, or Yearly.");
        }
    }
}
