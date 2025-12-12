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
        private readonly ExpenseContext _context;
        private readonly JwtService _jwtService;

        // Constructor
        public ExpenseService(ExpenseContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<ExpenseDTO> CreateExpenseAsync(int userId, ExpenseCreateDTO newExpense)
        {
            if (!isAmountValid(newExpense.Amount) || !isDateValid(newExpense.Date))
            {
                return null; // Invalid data
            }

            var expenseEntity = new Expense
            {
                UserId = userId,
                Amount = newExpense.Amount,
                Category = newExpense.Category,
                Date = newExpense.Date.ToDateTime(TimeOnly.MinValue)
            };

            _context.Expenses.Add(expenseEntity);
            await _context.SaveChangesAsync();

            return new ExpenseDTO
            {
                Id = expenseEntity.Id,
                Amount = expenseEntity.Amount,
                Category = expenseEntity.Category,
                Date = DateOnly.FromDateTime(expenseEntity.Date)
            };
        }

        public async Task<bool> DeleteExpenseAsync(int expenseId, int userId)
        {
            if (await GetExpenseByIdAsync(expenseId, userId) == null)
            {
                return false;
            }

            var expense = await _context.Expenses
                .Where(e => e.UserId == userId && e.Id == expenseId)
                .FirstOrDefaultAsync();

            if (expense == null)
            {
                return false;
            }

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ExpenseDTO> GetExpenseByIdAsync(int expenseId, int userId)
        {
            var expense = await _context.Expenses
                .Where(e => e.UserId == userId && e.Id == expenseId)
                .FirstOrDefaultAsync();

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
            return await _context.Expenses
                .Where(e => e.UserId == userId)
                .Select(e => new ExpenseDTO
                {
                    Id = e.Id,
                    Amount = e.Amount,
                    Category = e.Category,
                    Date = DateOnly.FromDateTime(e.Date)
                }).ToListAsync();
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
            return await _context.Expenses
                .Where(e => e.UserId == userId && e.Date.Year == year && e.Date.Month == month)
                .SumAsync(e => e.Amount);
        }

        public async Task<bool> UpdateExpenseAsync(int expenseId, int userId, ExpenseUpdateDTO updatedExpense)
        {
            var expense = await _context.Expenses
                .Where(e => e.UserId == userId && e.Id == expenseId)
                .FirstOrDefaultAsync();

            if (expense == null)
            {
                return false;
            }

            if (!isAmountValid(updatedExpense.Amount) || !isDateValid(updatedExpense.Date))
            {
                return false; // Invalid data
            }

            expense.Update(updatedExpense.Amount, updatedExpense.Date.ToDateTime(TimeOnly.MinValue), updatedExpense.Category);
            _context.Expenses.Update(expense);
            await _context.SaveChangesAsync();

            return true;
        }

        public bool isAmountValid(decimal amount)
        {
            return amount > 0;
        }

        public bool isDateValid(DateOnly date)
        {
            return date <= DateOnly.FromDateTime(DateTime.Now);
        }
    }
}