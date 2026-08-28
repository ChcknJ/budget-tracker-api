using FluentValidation;
using BudgetTracker.Features.Auth.DTOs;

namespace BudgetTracker.Features.Auth.Validators
{
    public class RefreshRequestValidator: AbstractValidator<RefreshRequest>
    {
        public RefreshRequestValidator()
        {
            RuleFor(x => x.RefreshToken).NotEmpty();
        }
    }
}
