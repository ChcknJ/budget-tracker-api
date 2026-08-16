using BudgetTracker.DTO;
using BudgetTracker.Validators;
using FluentValidation.TestHelper;

namespace BudgetTracker.Tests
{
    public class ExpenseQueryValidatorTest
    {
        [Fact]
        public void ExpenseQueryValidator_ValidRequest_ShouldPassValidation()
        {
            var query = new ExpenseQueryRequest
            {
                CategoryId = Guid.NewGuid(),
                SubscriptionId = Guid.NewGuid(),
                FromDate = DateOnly.FromDateTime(DateTime.Today),
                ToDate = DateOnly.FromDateTime(DateTime.Today),
                MinimumAmount = 100,
                MaximumAmount = 200,
                PageNumber = 1,
                PageSize = 20,
                SortBy = "date",
                SortOrder = "ascending",
            };

            var validator = new ExpenseQueryRequestValidator();

            var result = validator.TestValidate(query);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void ExpenseQueryValidator_MinimumAmountGreaterThanMaximumAmount_ShouldHaveValidationError()
        {
            var query = new ExpenseQueryRequest
            {
                CategoryId = Guid.NewGuid(),
                SubscriptionId = Guid.NewGuid(),
                FromDate = DateOnly.FromDateTime(DateTime.Today),
                ToDate = DateOnly.FromDateTime(DateTime.Today),
                MinimumAmount = 300,
                MaximumAmount = 200,
                PageNumber = 1,
                PageSize = 20,
                SortBy = "date",
                SortOrder = "ascending",
            };

            var validator = new ExpenseQueryRequestValidator();

            var result = validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(x=>x.MinimumAmount);
        }

        [Fact]
        public void ExpenseQueryValidator_InvalidSortBy_ShouldHaveValidationError()
        {
            var query = new ExpenseQueryRequest
            {
                CategoryId = Guid.NewGuid(),
                SubscriptionId = Guid.NewGuid(),
                FromDate = DateOnly.FromDateTime(DateTime.Today),
                ToDate = DateOnly.FromDateTime(DateTime.Today),
                MinimumAmount = 100,
                MaximumAmount = 200,
                PageNumber = 1,
                PageSize = 20,
                SortBy = "Date",
                SortOrder = "ascending",
            };

            var validator = new ExpenseQueryRequestValidator();

            var result = validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(x => x.SortBy);
        }

        [Fact]
        public void ExpenseQueryValidator_InvalidSortOrder_ShouldHaveValidationError()
        {
            var query = new ExpenseQueryRequest
            {
                CategoryId = Guid.NewGuid(),
                SubscriptionId = Guid.NewGuid(),
                FromDate = DateOnly.FromDateTime(DateTime.Today),
                ToDate = DateOnly.FromDateTime(DateTime.Today),
                MinimumAmount = 100,
                MaximumAmount = 200,
                PageNumber = 1,
                PageSize = 20,
                SortBy = "date",
                SortOrder = "Ascending",
            };

            var validator = new ExpenseQueryRequestValidator();

            var result = validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(x => x.SortOrder);
        }

        [Fact]
        public void ExpenseQueryValidator_PageSizeOver20_ShouldHaveValidationError()
        {
            var query = new ExpenseQueryRequest
            {
                CategoryId = Guid.NewGuid(),
                SubscriptionId = Guid.NewGuid(),
                FromDate = DateOnly.FromDateTime(DateTime.Today),
                ToDate = DateOnly.FromDateTime(DateTime.Today),
                MinimumAmount = 100,
                MaximumAmount = 200,
                PageNumber = 1,
                PageSize = 30,
                SortBy = "date",
                SortOrder = "ascending",
            };

            var validator = new ExpenseQueryRequestValidator();

            var result = validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(x => x.PageSize);
        }


        [Fact]
        public void ExpenseQueryValidator_PageNumberLessThan1_ShouldHaveValidationError()
        {
            var query = new ExpenseQueryRequest
            {
                CategoryId = Guid.NewGuid(),
                SubscriptionId = Guid.NewGuid(),
                FromDate = DateOnly.FromDateTime(DateTime.Today),
                ToDate = DateOnly.FromDateTime(DateTime.Today),
                MinimumAmount = 100,
                MaximumAmount = 200,
                PageNumber = 0,
                PageSize = 20,
                SortBy = "date",
                SortOrder = "ascending",
            };

            var validator = new ExpenseQueryRequestValidator();

            var result = validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(x => x.PageNumber);
        }
    }
}
