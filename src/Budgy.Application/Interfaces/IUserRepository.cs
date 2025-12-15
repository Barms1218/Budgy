using Budgy.Domain.Entities;

namespace Budgy.Application.Interfaces
{
    public interface IUserRepository
    {
        // Define user-related business operations here
        // Want to be able to find a particular user by their username
        Task<User?> GetUserByUsernameAsync(string username);

        // Want to be able to find a particular user by their ID
        Task<User?> GetUserByIdAsync(int userId);

        // Create a new user
        Task<User> RegisterAsync(UserRegisterDTO newUser);

        // Want to be able to change a user's username
        Task<bool> UpdateUsernameAsync(int userId, string newUsername);

        // Want to be able to delete a user
        Task<bool> DeleteUserAsync(int userId);

        Task<bool> UpdatePasswordAsync(int userId, string newPassword);
    }
}