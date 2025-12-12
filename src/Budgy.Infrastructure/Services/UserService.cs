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

        /// <summary>
        /// Deletes a user by their ID.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets a user by their ID.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets a user by their username.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Updates a user's password.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public Task<bool> UpdatePasswordAsync(int userId, string newPassword)
        {
            var user = _context.Users.FirstOrDefaultAsync(u => u.Id == userId).Result;

            if (user == null)
            {
                return Task.FromResult(false);
            }

            user.PasswordHash = HashPassword(newPassword);
            _context.SaveChanges();

            return Task.FromResult(true);
        }

        /// <summary>
        /// Updates a user's username.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newUsername"></param>
        /// <returns></returns>
        public Task<bool> UpdateUsernameAsync(int userId, string newUsername)
        {
            var user = _context.Users.FirstOrDefaultAsync(u => u.Id == userId).Result;

            if (user == null)
            {
                return Task.FromResult(false);
            }

            user.UserName = newUsername;
            _context.SaveChanges();

            return Task.FromResult(true);
        }

        /// <summary>
        /// Hashes a password using BCrypt.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}