using System.Security.Claims;
using Budgy.Application.DTOs;

namespace Budgy.Application.Interfaces
{
    /// <summary>
    /// Defines the interface for the JWT service in the Expense Tracker application.
    /// </summary>
    public interface IJwtService
    {
        // Define methods for generating and validating JWTs here
        string GenerateToken(int userId, string username);

        ClaimsPrincipal? ValidateToken(string token);

        // Find the user by their token
        Task<UserDTO?> GetUserFromTokenAsync(string token);

        // Log in a user and return their DTO
        Task<UserDTO?> Login(UserLoginDTO userLoginDTO);
        
    }
}