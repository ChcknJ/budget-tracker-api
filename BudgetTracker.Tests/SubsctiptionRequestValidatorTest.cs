using BudgetTracker.DTO;
using BudgetTracker.Validators;
using FluentValidation.TestHelper;

namespace BudgetTracker.Tests
{
    public class SubsctiptionRequestValidatorTest
    {
        [Fact]
        public void SubscriptionRequestValidator_ValidSubscriptionRequest_ShouldPassValidation()
        {
            var request = new SubscriptionRequest
            {
                CategoryId = Guid.NewGuid(),
                Name = "Sample",
                Amount = 10000,
                StartDate = DateOnly.FromDateTime(DateTime.Today),
                EndDate = DateOnly.FromDateTime(DateTime.Today),
                BillingCycle = "Monthly"
            };

            var validator = new SubscriptionRequestValidator();

            var result = validator.TestValidate(request);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void SubscriptionRequestValidator_AmountLessThan1_ShouldHaveValidationError()
        {
            var request = new SubscriptionRequest
            {
                CategoryId = Guid.NewGuid(),
                Name = "Sample",
                Amount = 0,
                StartDate = DateOnly.FromDateTime(DateTime.Today),
                EndDate = DateOnly.FromDateTime(DateTime.Today),
                BillingCycle = "Monthly"
            };

            var validator = new SubscriptionRequestValidator();

            var result = validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x=>x.Amount);
        }

        [Fact]
        public void SubscriptionRequestValidator_InvalidBillingCycle_ShouldHaveValidationError()
        {
            var request = new SubscriptionRequest
            {
                CategoryId = Guid.NewGuid(),
                Name = "Sample",
                Amount = 100,
                StartDate = DateOnly.FromDateTime(DateTime.Today),
                EndDate = DateOnly.FromDateTime(DateTime.Today),
                BillingCycle = "Daily"
            };

            var validator = new SubscriptionRequestValidator();

            var result = validator.TestValidate(request);

            result.ShouldHaveValidationErrorFor(x => x.BillingCycle);
        }
    }
}
