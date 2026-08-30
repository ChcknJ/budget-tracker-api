using BudgetTracker.Features.Auth.Validators;
using BudgetTracker.Features.Auth.DTOs;
public class RefreshRequestValidatorTests
{
    private readonly RefreshRequestValidator _validator = new();

    [Fact]
    public void NonEmptyToken_PassesValidation()
    {
        var result = _validator.Validate(new RefreshRequest("some-token-value"));

        Assert.True(result.IsValid);
    }

    [Fact]
    public void EmptyToken_FailsValidation()
    {
        var result = _validator.Validate(new RefreshRequest(""));

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "RefreshToken");
    }
}