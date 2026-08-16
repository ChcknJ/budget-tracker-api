using FluentValidation.TestHelper;
using BudgetTracker.DTO;
using BudgetTracker.Validators;

namespace BudgetTracker.Tests
{
    public class ExpenseRequestValidatorTest
    {
        [Fact]
        public void ExpenseRequestValidator_ValidRequest_ShouldPassValidation()
        {
            var request = new ExpenseRequest
            {
                CategoryId = Guid.NewGuid(),
                Amount = 100,
                Description = "Lunch",
                Date = DateOnly.FromDateTime(DateTime.Today)
            };

            var validator = new ExpenseRequestValidator();

            var result = validator.TestValidate(request);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void ExpenseRequestValidator_AmountLessThan1_ShouldHaveValidationError()
        {
            var request = new ExpenseRequest
            {
                CategoryId = Guid.NewGuid(),
                Amount = 0,
                Description = "Dinner",
                Date = DateOnly.FromDateTime(DateTime.Today)
            };

            var validator = new ExpenseRequestValidator();

            var result = validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x=>x.Amount);
        }

        [Fact]
        public void ExpenseRequestValidator_DescriptionOver100Characters_ShouldHaveValidationError()
        {
            var request = new ExpenseRequest
            {
                CategoryId = Guid.NewGuid(),
                Amount = 100,
                Description = new string('a', 101),
                Date = DateOnly.FromDateTime(DateTime.Today)
            };

            var validator = new ExpenseRequestValidator();

            var result = validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x=>x.Description);
        }
    }
}
