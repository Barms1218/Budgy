using Xunit;
using Moq;
using System;
using System.Threading.Tasks;
using Budgy.Application.Interfaces;
using Budgy.Application.DTOs;

namespace Budgy.Application.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<IJwtService> _jwtServiceMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _jwtServiceMock = new Mock<IJwtService>();
            _userService = new UserService(_jwtServiceMock.Object);
        }

        // Test methods would go here

        [Fact]
        public async Task GetUserFromTokenAsync_ValidToken_ReturnsUserDTO()
        {
            // Arrange
            var token = "valid.jwt.token";
            var expectedUser = new UserDTO
            {
                Id = 1,
                UserName = "testuser",
                Expenses = new List<Expense>
                {
                    new Expense { Id = 1, Amount = 100, Description = "Groceries" },
                    new Expense { Id = 2, Amount = 50, Description = "Transport" }
                }
            };

            _jwtServiceMock.Setup(s => sbyte.GetUserFromTokenAsync(token))
                .ReturnsAsync(expectedUser);

            // Act
            var result = await _expenseService.GetUserFromTokenAsync(token);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedUser.Id, result.Id);
            Assert.Equal(expectedUser.UserName, result.UserName);
            Assert.Equal(expectedUser.Expenses.Count, result.Expenses.Count);

        }
    }
}