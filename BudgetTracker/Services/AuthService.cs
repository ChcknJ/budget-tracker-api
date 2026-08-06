using BudgetTracker.Interfaces;
using BudgetTracker.DTO.Request;
using BudgetTracker.Models;
using BudgetTracker.Database;
using Microsoft.EntityFrameworkCore;
namespace BudgetTracker.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;

        public AuthService (AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> RegisterAsync(RegisterRequest registerRequest)
        {
            // chech if account already existing
            var checkUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == registerRequest.Username);

            // if user is not null means user already has account
            if (checkUser != null)
            {
                return false;
            }

            // hash password
            string hashPass = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password);

            // create user
            var user = new User
            {
                Username = registerRequest.Username,
                HashPassword = hashPass
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return true;
        }  

        public async Task<bool> LoginAsync(LoginRequest loginRequest)
        {
            // look for the user if it is in the db
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginRequest.Username);

            if (user == null)
            {
                return false;
            }

            return BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.HashPassword);
            
        }
    }
}
