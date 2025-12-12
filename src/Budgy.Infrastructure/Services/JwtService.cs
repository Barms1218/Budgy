using System.Security.Claims;
using Budgy.Application.DTOs;
using Budgy.Application.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;


namespace Budgy.Infrastructure.Services
{
    /// <summary>
    /// Implements the JWT service for generating and validating JSON Web Tokens in the Expense Tracker application.
    /// </summary>
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly ExpenseContext _context;

        public JwtService(IConfiguration configuration, ExpenseContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        
        // Implementation of JWT service methods would go here
        public string GenerateToken(int userId, string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured"));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    new Claim(ClaimTypes.Name, username)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public Task<UserDTO?> GetUserFromTokenAsync(string token)
        {
            var principal = ValidateToken(token);
            if (principal == null)
                return Task.FromResult<UserDTO?>(null);

            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Task.FromResult<UserDTO?>(null);
            
            if (!int.TryParse(userIdClaim.Value, out int userId))
                return Task.FromResult<UserDTO?>(null);
            
            return _context.Users
                .Where(u => u.Id == userId)
                .Select(u => new UserDTO
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Expenses = u.Expenses
                }).FirstOrDefaultAsync();
        }

        public Task<UserDTO?> Login(UserLoginDTO userLoginDTO)
        {
            throw new NotImplementedException();
        }

        public ClaimsPrincipal? ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured"));

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return principal;
                
            }
            catch
            {
                return null;
            }
        }
    }
}