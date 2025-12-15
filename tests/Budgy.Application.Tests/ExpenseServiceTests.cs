using Xunit;
using Moq;
using System;
using System.Threading.Tasks;
using Budgy.Application.Interfaces;
using Budgy.Application.DTOs;

namespace Budgy.Application.Tests
{
    public class ExpenseServiceTests
    {
        private readonly Mock<IJwtService> _jwtServiceMock;

        private readonly ExpenseService _expenseService;

        public ExpenseServiceTests()
        {
            _jwtServiceMock = new Mock<IJwtService>();
            _expenseService = new ExpenseService(_jwtServiceMock.Object);
        }


        #region Test Methods

        [Fact]
        public async Task GetExpenseByIdAsync_ValidId_ReturnsExpenseDTO()
        {
            // Arrange
            var expenseId = 1;
            var userId = 1;
            var expectedExpense = new ExpenseDTO
            {
                Id = expenseId,
                Amount = 100.0m,
                Category = "Food",
                DateOnly = DateOnly.FrameDateTime(2024, 1, 1)
            };

            // Act
            var result = await _expenseService.GetExpenseByIdAsync(expenseId, userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedExpense.Id, result.Id);
            Assert.Equal(expectedExpense.Amount, result.Amount);
            Assert.Equal(expectedExpense.Category, result.Category);
            Assert.Equal(expectedExpense.Date, result.Date);
        }

        #endregion
    }
}
