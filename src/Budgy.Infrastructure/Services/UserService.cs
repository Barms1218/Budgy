using Budgy.Application.DTOs;
using Budgy.Application.Interfaces;

namespace Budgy.Infrastructure.Services
{
    public class UserService : IUserService
    {
        // Implementation of IUserService methods
        public Task<bool> DeleteUserAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<UserDTO> GetUserByIdAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<UserDTO> GetUserByUsernameAsync(string username)
        {
            throw new NotImplementedException();
        }

        public Task<UserDTO> RegisterAsync(UserRegisterDTO newUser)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateUsernameAsync(int userId, string newUsername)
        {
            throw new NotImplementedException();
        }
    }
}