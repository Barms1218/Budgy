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
        private readonly IExpenseService _expenseService;

        private readonly Mock<IExpenseRepository> _expenseRepositoryMock;

        public ExpenseServiceTests()
        {
            _expenseRepositoryMock = new Mock<IExpenseRepository>();
            _expenseService = new ExpenseService(_expenseRepositoryMock.Object);
        }


        #region Test Methods

        /// <summary>
        /// Tests the GetExpenseByIdAsync method with a valid expense ID.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Tests the CreateExpenseAsync method with valid data.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CreateExpenseAsync_ValidData_ReturnsExpenseDTO()
        {
            // Arrange
            var userId = 1;
            var newExpense = new ExpenseCreateDTO
            {
                Amount = 150.0m,
                Category = "Transport",
                DateOnly = DateOnly.FrameDateTime(2025, 1, 1)
            };

            // Act
            var result = await _expenseService.CreateExpenseAsync(userId, newExpense);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newExpense.Amount, result.Amount);
            Assert.Equal(newExpense.Category, result.Category);
            Assert.Equal(newExpense.Date, result.Date);
        }

        /// <summary>
        /// Tests the DeleteExpenseAsync method with a valid expense ID.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DeleteExpenseAsync_ValidId_ReturnsTrue()
        {
            // Arrange
            var expenseId = 1;
            var userId = 1;

            var newExpense = new ExpenseCreateDTO
            {
                Amount = 150.0m,
                Category = "Transport",
                DateOnly = DateOnly.FrameDateTime(2025, 1, 1)
            };

            await _expenseService.DeleteExpenseAsync(expenseId, userId);

            // Act
            var result = await _expenseService.DeleteExpenseAsync(expenseId, userId);

            // Assert
            Assert.True(result);
        }

        #endregion
    }
}
