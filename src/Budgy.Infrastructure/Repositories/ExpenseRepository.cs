using Budgy.Application.Interfaces;
using Budgy.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Budgy.Infrastructure.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly ExpenseContext _context;

        public ExpenseRepository(ExpenseContext context)
        {
            _context = context;
        }

        public Task<Expense> CreateExpenseAsync(int userId, ExpenseCreateDTO newExpense)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteExpenseAsync(int expenseId, int userId)
        {
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

        public async Task<Expense?> GetExpenseByIdAsync(int expenseId, int userId)
        {
            return await _context.Expenses
                .Where(e => e.UserId == userId && e.Id == expenseId)
                .FirstOrDefaultAsync();
        }

        public Task<IEnumerable<Expense>> GetExpensesByUserIdAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<decimal> GetTotalExpensesForMonthAsync(int userId, int year, int month)
        {
            return _context.Expenses
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

            if (!expense.isAmountValid(updatedExpense.Amount) || !expense.isDateValid(updatedExpense.Date.ToDateTime(TimeOnly.MinValue)))
            {
                return false; // Invalid data
            }

            expense.Update(updatedExpense.Amount, updatedExpense.Date.ToDateTime(TimeOnly.MinValue), updatedExpense.Category);
            _context.Expenses.Update(expense);
            await _context.SaveChangesAsync();
            return true;
        }

        // Implementation of CRUD methods would go here
    }
}