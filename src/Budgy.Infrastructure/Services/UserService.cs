using Budgy.Application.DTOs;
using Budgy.Application.Interfaces;
using Budgy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace Budgy.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly ExpenseContext _context;

        public UserService(ExpenseContext context)
        {
            _context = context;
        }

        // Implementation of IUserService methods
        public Task<bool> DeleteUserAsync(int userId)
        {
            var user = _context.Users.FirstOrDefaultAsync(u => u.Id == userId).Result;

            if (user == null)
            {
                return Task.FromResult(false);
            }

            _context.Users.Remove(user);
            _context.SaveChanges();

            return Task.FromResult(true);
        }

        public Task<UserDTO?> GetUserByIdAsync(int userId)
        {
            return _context.Users
                .Where(u => u.Id == userId)
                .Select(u => new UserDTO
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Expenses = u.Expenses
                }).FirstOrDefaultAsync();
        }

        public Task<UserDTO?> GetUserByUsernameAsync(string username)
        {
            return _context.Users
                .Where(u => u.UserName == username)
                .Select(u => new UserDTO
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Expenses = u.Expenses
                }).FirstOrDefaultAsync();
        }

        public Task<UserDTO> RegisterAsync(UserRegisterDTO newUser)
        {
            User user = new User
            {
                UserName = newUser.UserName,
                PasswordHash = HashPassword(newUser.Password),
                Expenses = new List<Expense>()
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return Task.FromResult(new UserDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                Expenses = user.Expenses
            });
        }

        public Task<bool> UpdatePasswordAsync(int userId, string newPassword)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateUsernameAsync(int userId, string newUsername)
        {
            throw new NotImplementedException();
        }

        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}