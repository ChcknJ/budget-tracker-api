using BudgetTracker.Features.Auth.Validators;
using BudgetTracker.Features.Auth.DTOs;
public class LoginRequestValidatorTests
{
    private readonly LoginRequestValidator _validator = new();

    [Fact]
    public void ValidRequest_PassesValidation()
    {
        var request = new LoginRequest("eisk@test.com", "anything");

        var result = _validator.Validate(request);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void WeakPassword_StillPasses_NoComplexityRulesOnLogin()
    {
        var request = new LoginRequest("eisk@test.com", "a");

        var result = _validator.Validate(request);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void EmptyPassword_FailsValidation()
    {
        var request = new LoginRequest("eisk@test.com", "");

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Password");
    }

    [Theory]
    [InlineData("")]
    [InlineData("not-an-email")]
    public void InvalidEmail_FailsValidation(string email)
    {
        var request = new LoginRequest(email, "anything");

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "EmailAddress");
    }
}