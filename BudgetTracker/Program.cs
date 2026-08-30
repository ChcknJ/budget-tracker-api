using BudgetTracker.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.RateLimiting;
using BudgetTracker.Exceptions;
using BudgetTracker.Features.Auth.Interfaces;
using BudgetTracker.Features.Auth.Services;
using BudgetTracker.Features.Categories.Interfaces;
using BudgetTracker.Features.Categories.Services;
using BudgetTracker.Features.Expenses.Interfaces;
using BudgetTracker.Features.Expenses.Services;
using BudgetTracker.Features.Subscriptions.Interfaces;
using BudgetTracker.Features.Subscriptions.Services;
using BudgetTracker.Features.Budgets.Interfaces;
using BudgetTracker.Features.Budgets.Services;
using BudgetTracker.Features.Summary.Services;
using BudgetTracker.Features.Users.Interfaces;
using BudgetTracker.Features.Users.Services;
using BudgetTracker.Database.Configs;
using BudgetTracker.Features.Summary.Interfaces;
using BudgetTracker.Features.Auth.Validators;
using BudgetTracker.Filters;

namespace BudgetTracker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Database Config
            builder.Services.AddDbContext<AppDbContext>(option =>
            {
                option.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")).UseSnakeCaseNamingConvention();
            });

            // Jwt Config
            builder.Services.Configure<JwtSettings>(
                builder.Configuration.GetSection("Jwt"));

            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
                        ),

                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],

                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["Jwt:Audience"],

                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            // Services
            // Auth
            builder.Services.AddScoped<IWriteAuthServices, AuthService>();
            //Expense
            builder.Services.AddScoped<IReadExpenseService, ExpenseService>();
            builder.Services.AddScoped<IWriteExpenseService, ExpenseService>();
            //Category
            builder.Services.AddScoped<IReadCategoryService, CategoryService>();
            builder.Services.AddScoped<IWriteCategoryService, CategoryService>();
            //Subscription
            builder.Services.AddScoped<IReadSubscriptionService, SubscriptionService>();
            builder.Services.AddScoped<IWriteSubscriptionService, SubscriptionService>();
            //Budget
            builder.Services.AddScoped<IReadBudgetService, BudgetService>();
            builder.Services.AddScoped<IWriteBudgetService, BudgetService>();
            //Summary
            builder.Services.AddScoped<ISummaryService, SummaryService>();
            //User
            builder.Services.AddScoped<IReadUserService, UserService>();
            builder.Services.AddScoped<IWriteUserService, UserService>();


            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

            // Validation
            builder.Services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();

            // Add services to the container.

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<ValidationFilter>();
            });



            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            // rate limiter
            builder.Services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                options.AddFixedWindowLimiter("General", limiterOptions =>
                {
                    limiterOptions.PermitLimit = 10;
                    limiterOptions.Window = TimeSpan.FromMinutes(1);
                });

                options.AddFixedWindowLimiter("Auth", limiterOptions =>
                {
                    limiterOptions.PermitLimit = 5;
                    limiterOptions.Window = TimeSpan.FromMinutes(1);
                });
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseExceptionHandler(options => { });

            app.UseHttpsRedirection();

            app.UseRateLimiter();

            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
