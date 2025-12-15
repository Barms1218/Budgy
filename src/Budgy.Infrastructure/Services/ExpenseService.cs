using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Budgy.Application.Interfaces;
using Budgy.Application.DTOs;
using Budgy.Domain.Entities;
using SQLitePCL;

namespace Budgy.Infrastructure.Services
{
    /// <summary>
    /// Implements the expense service for managing expenses in the Expense Tracker application.
    /// </summary>
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;

        // Constructor
        public ExpenseService(IExpenseRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        /// <summary>
        /// Creates a new expense for a user.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newExpense"></param>
        /// <returns></returns>
        public async Task<ExpenseDTO> CreateExpenseAsync(int userId, ExpenseCreateDTO newExpense)
        {
            var expense = await _expenseRepository.CreateExpenseAsync(userId, newExpense);

            return new ExpenseDTO
            {
                Id = expense.Id,
                Amount = expense.Amount,
                Category = expense.Category,
                Date = DateOnly.FromDateTime(expense.Date)
            };
        }

        public async Task<bool> DeleteExpenseAsync(int expenseId, int userId)
        {
            if (await GetExpenseByIdAsync(expenseId, userId) == null)
            {
                return false;
            }

            return await _expenseRepository.DeleteExpenseAsync(expenseId, userId);
        }

        /// <summary>
        /// Retrieves a specific expense by its ID for a given user.
        /// </summary>
        /// <param name="expenseId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ExpenseDTO> GetExpenseByIdAsync(int expenseId, int userId)
        {
            var expense = await _expenseRepository.GetExpenseByIdAsync(expenseId, userId);

            if (expense == null)
            {
                return null;
            }

           return new ExpenseDTO
           {
               Id = expense.Id,
               Amount = expense.Amount,
               Category = expense.Category,
               Date = DateOnly.FromDateTime(expense.Date)
           };
        }

        /// <summary>
        /// Retrieves all expenses for a specific user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ExpenseDTO>> GetExpensesByUserIdAsync(int userId)
        {
            return (await _expenseRepository.GetExpensesByUserIdAsync(userId))
                .Select(expense => new ExpenseDTO
                {
                    Id = expense.Id,
                    Amount = expense.Amount,
                    Category = expense.Category,
                    Date = DateOnly.FromDateTime(expense.Date)
                });
        }

        /// <summary>
        /// Calculates the total expenses for a user in a given month and year.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public async Task<decimal> GetTotalExpensesForMonthAsync(int userId, int year, int month)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            return await _expenseRepository.GetTotalExpensesForMonthAsync(userId, year, month);
        }

        /// <summary>
        /// Updates an existing expense for a user.
        /// </summary>
        /// <param name="expenseId"></param>
        /// <param name="userId"></param>
        /// <param name="updatedExpense"></param>
        /// <returns></returns>
        public async Task<bool> UpdateExpenseAsync(int expenseId, int userId, ExpenseUpdateDTO updatedExpense)
        {
            return await _expenseRepository.UpdateExpenseAsync(expenseId, userId, updatedExpense);
        }
    }
}