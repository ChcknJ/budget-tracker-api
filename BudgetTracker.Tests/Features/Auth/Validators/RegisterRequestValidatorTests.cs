using BudgetTracker.Features.Auth.Validators;
using BudgetTracker.Features.Auth.DTOs;

public class RegisterRequestValidatorTests
{
    private readonly RegisterRequestValidator _validator = new();

    [Fact]
    public void ValidRequest_PassesValidation()
    {
        var request = new RegisterRequest("eisk", "Password1", "eisk@test.com");

        var result = _validator.Validate(request);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void EmptyUsername_FailsWithNotEmptyAndMinLength()
    {
        var request = new RegisterRequest("", "Password1", "eisk@test.com");

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Username" && e.ErrorCode == "NotEmptyValidator");
        Assert.Contains(result.Errors, e => e.PropertyName == "Username" && e.ErrorCode == "MinimumLengthValidator");
    }

    [Theory]
    [InlineData("ab")]              // too short
    [InlineData("this-username-is-way-too-long-for-the-rule")] // too long
    public void UsernameOutsideLengthBounds_FailsValidation(string username)
    {
        var request = new RegisterRequest(username, "Password1", "eisk@test.com");

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Username");
    }

    [Theory]
    [InlineData("not-an-email")]
    [InlineData("")]
    [InlineData("missing-at-sign.com")]
    public void InvalidEmail_FailsValidation(string email)
    {
        var request = new RegisterRequest("eisk", "Password1", email);

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "EmailAddress");
    }

    [Fact]
    public void TooShortPassword_FailsMinimumLength()
    {
        var request = new RegisterRequest("eisk", "Aa1", "eisk@test.com");

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Password" && e.ErrorCode == "MinimumLengthValidator");
    }

    [Fact]
    public void PasswordWithoutUppercase_FailsRegex()
    {
        var request = new RegisterRequest("eisk", "password1", "eisk@test.com");

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Password" && e.ErrorMessage.Contains("uppercase"));
    }

    [Fact]
    public void PasswordWithoutDigit_FailsRegex()
    {
        var request = new RegisterRequest("eisk", "PasswordOnly", "eisk@test.com");

        var result = _validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Password" && e.ErrorMessage.Contains("digit"));
    }
}