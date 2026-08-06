using BudgetTracker.Database;
using BudgetTracker.Interfaces;
using BudgetTracker.Services;
using EFCore.NamingConventions;
using Microsoft.EntityFrameworkCore;

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

            // Services
            builder.Services.AddScoped<IAuthService, AuthService>();

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
