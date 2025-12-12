using System.Security.Claims;
using Budgy.Application.DTOs;
using Budgy.Application.Interfaces;

namespace Budgy.Infrastructure.Services
{
    /// <summary>
    /// Implements the JWT service for generating and validating JSON Web Tokens in the Expense Tracker application.
    /// </summary>
    public class JwtService : IJwtService
    {
        // Implementation of JWT service methods would go here
        public string GenerateToken(int userId, string username)
        {
            throw new NotImplementedException();
        }

        public Task<UserDTO?> GetUserFromTokenAsync(string token)
        {
            throw new NotImplementedException();
        }

        public Task<UserDTO?> Login(UserLoginDTO userLoginDTO)
        {
            throw new NotImplementedException();
        }

        public ClaimsPrincipal? ValidateToken(string token)
        {
            throw new NotImplementedException();
        }
    }
}