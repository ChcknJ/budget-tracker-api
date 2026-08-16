using BudgetTracker.DTO;
using FluentValidation;


namespace BudgetTracker.Validators
{
    public class ExpenseRequestValidator : AbstractValidator<ExpenseRequest>
    {
        public ExpenseRequestValidator()
        {
            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Category is required.");
            RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Amount must be greater than 0.");
            RuleFor(x => x.Description).MaximumLength(100).WithMessage("Description cannot exceed 100 characters.");
            RuleFor(x => x.Date).NotEmpty().WithMessage("Date is required");

            
        }
    }
}
