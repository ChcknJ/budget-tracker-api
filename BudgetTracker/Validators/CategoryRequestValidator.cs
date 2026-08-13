using BudgetTracker.DTO;
using FluentValidation;

namespace BudgetTracker.Validators

{
    public class CategoryRequestValidator : AbstractValidator<CategoryRequest>
    {
        public CategoryRequestValidator()
        {
            RuleFor(x => x.Name).MaximumLength(100).WithMessage("Name cannot exceed 100 characters.").Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("Name cannot be empty or whitespace.");
            
        }
    }
}
