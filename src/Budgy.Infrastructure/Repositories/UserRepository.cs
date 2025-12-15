using Budgy.Application.Interfaces;
using Budgy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace Budgy.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ExpenseContext _context;

        public UserRepository(ExpenseContext context)
        {
            _context = context;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return false;
            }
            _context.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _context.Users
            .Where(u => u.Id == userId)
            .FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .Where(u => u.UserName == username)
                .FirstOrDefaultAsync();
        }

        public async Task<User> RegisterAsync(UserRegisterDTO newUser)
        {
            var user = new User
            {
                UserName = newUser.UserName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(newUser.Password),
                Expenses = new List<Expense>()
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            
            return user;
        }

        public Task<bool> UpdatePasswordAsync(int userId, string newPassword)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateUsernameAsync(int userId, string newUsername)
        {
            throw new NotImplementedException();
        }

        // Implement user-related data operations here
    }
}