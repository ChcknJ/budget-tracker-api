using BudgetTracker.Database;
using BudgetTracker.Database.Configs;
using BudgetTracker.Features.Auth.DTOs;
using BudgetTracker.Features.Auth.Models;
using BudgetTracker.Features.Auth.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
public class AuthServiceTests
{
    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    private static AuthService CreateService(AppDbContext context)
    {
        var jwtSettings = Options.Create(new JwtSettings
        {
            Key = "test-key-thats-long-enough-for-hmac-sha256",
            Issuer = "test",
            Audience = "test",
            ExpiryMinutes = 15,
            RefreshTokenExpiryDays = 7
        });
        return new AuthService(context, jwtSettings);
    }

    [Fact]
    public async Task RegisterAsync_NewEmail_ReturnsSuccess()
    {
        var context = CreateContext();
        var service = CreateService(context);

        var result = await service.RegisterAsync(
            new RegisterRequest("eisk", "Password1", "eisk@test.com"),
            CancellationToken.None);

        Assert.True(result.Success);
        Assert.NotNull(result.Response);
    }

    [Fact]
    public async Task RegisterAsync_DuplicateEmail_ReturnsConflictError()
    {
        var context = CreateContext();
        var service = CreateService(context);
        await service.RegisterAsync(new RegisterRequest("eisk", "Password1", "eisk@test.com"), CancellationToken.None);

        var result = await service.RegisterAsync(
            new RegisterRequest("someoneelse", "Password1", "eisk@test.com"),
            CancellationToken.None);

        Assert.False(result.Success);
        Assert.Equal(AuthErrorType.InvalidEmailAddress, result.ErrorType);
    }

    [Fact]
    public async Task LoginAsync_CorrectCredentials_ReturnsSuccess()
    {
        var context = CreateContext();
        var service = CreateService(context);
        await service.RegisterAsync(new RegisterRequest("eisk", "Password1", "eisk@test.com"), CancellationToken.None);

        var result = await service.LoginAsync(
            new LoginRequest("eisk@test.com", "Password1"),
            CancellationToken.None);

        // this exact assertion would have caught your earlier "Success: false" bug
        Assert.True(result.Success);
        Assert.NotNull(result.Response);
    }

    [Fact]
    public async Task LoginAsync_WrongPassword_And_NonexistentEmail_ReturnSameMessage()
    {
        var context = CreateContext();
        var service = CreateService(context);
        await service.RegisterAsync(new RegisterRequest("eisk", "Password1", "eisk@test.com"), CancellationToken.None);

        var wrongPassword = await service.LoginAsync(new LoginRequest("eisk@test.com", "WrongPass1"), CancellationToken.None);
        var noAccount = await service.LoginAsync(new LoginRequest("nobody@test.com", "WrongPass1"), CancellationToken.None);

        Assert.False(wrongPassword.Success);
        Assert.False(noAccount.Success);
        // this is the enumeration-leak regression test
        Assert.Equal(wrongPassword.ErrorMessage, noAccount.ErrorMessage);
    }

    [Fact]
    public async Task RefreshAsync_ValidToken_RotatesAndRevokesOldToken()
    {
        var context = CreateContext();
        var service = CreateService(context);
        var registerResult = await service.RegisterAsync(new RegisterRequest("eisk", "Password1", "eisk@test.com"), CancellationToken.None);
        var originalRefreshToken = registerResult.Response!.RefreshToken;

        var refreshResult = await service.RefreshAsync(new RefreshRequest(originalRefreshToken), CancellationToken.None);

        Assert.True(refreshResult.Success);
        Assert.NotEqual(originalRefreshToken, refreshResult.Response!.RefreshToken);

        var oldTokenHash = Convert.ToBase64String(SHA256.HashData(Convert.FromBase64String(originalRefreshToken)));
        var oldTokenInDb = await context.RefreshTokens.FirstAsync(t => t.TokenHash == oldTokenHash);
        Assert.NotNull(oldTokenInDb.RevokedAt);
        Assert.NotNull(oldTokenInDb.ReplacedById);
    }

    [Fact]
    public async Task RefreshAsync_ReusedRevokedToken_RevokesEntireTokenFamily()
    {
        var context = CreateContext();
        var service = CreateService(context);
        var registerResult = await service.RegisterAsync(new RegisterRequest("eisk", "Password1", "eisk@test.com"), CancellationToken.None);
        var originalToken = registerResult.Response!.RefreshToken;

        // rotate once — originalToken is now revoked, a new token exists
        var firstRefresh = await service.RefreshAsync(new RefreshRequest(originalToken), CancellationToken.None);
        var newActiveToken = firstRefresh.Response!.RefreshToken;

        // replay the already-revoked original token — should trigger family revocation
        var replayResult = await service.RefreshAsync(new RefreshRequest(originalToken), CancellationToken.None);
        Assert.False(replayResult.Success);

        // the token that was still valid a moment ago should now ALSO be revoked
        var secondRefreshAttempt = await service.RefreshAsync(new RefreshRequest(newActiveToken), CancellationToken.None);
        Assert.False(secondRefreshAttempt.Success);
    }
}