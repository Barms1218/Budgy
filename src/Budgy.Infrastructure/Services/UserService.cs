using Budgy.Application.DTOs;
using Budgy.Application.Interfaces;
using Budgy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace Budgy.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Deletes a user by their ID.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteUserAsync(int userId)
        {
            return await _userRepository.DeleteUserAsync(userId);
        }

        /// <summary>
        /// Gets a user by their ID.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<UserDTO?> GetUserByIdAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                return null;
            }

            return new UserDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                Expenses = user.Expenses
            };
        }

        /// <summary>
        /// Gets a user by their username.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<UserDTO?> GetUserByUsernameAsync(string username)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);

            if (user == null)
            {
                return null;
            }

            return new UserDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                Expenses = user.Expenses
            };
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>
        public async Task<UserDTO> RegisterAsync(UserRegisterDTO newUser)
        {
            var user = await _userRepository.RegisterAsync(newUser);

            return new UserDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                Expenses = user.Expenses
            };
        }

        /// <summary>
        /// Updates a user's password.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public async Task<bool> UpdatePasswordAsync(int userId, string newPassword)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                return false;
            }

            return await _userRepository.UpdatePasswordAsync(userId, HashPassword(newPassword));
            
        }

        /// <summary>
        /// Updates a user's username.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newUsername"></param>
        /// <returns></returns>
        public async Task<bool> UpdateUsernameAsync(int userId, string newUsername)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                return false;
            }

            return await _userRepository.UpdateUsernameAsync(userId, newUsername);
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