using Budgy.Domain.Entities;


namespace Budgy.Application.Interfaces
{
    public interface IExpenseRepository
    {
        // C - Create
        // Returns the newly created expense object (or its ID)
        Task<Expense> CreateExpenseAsync(int userId, ExpenseCreateDTO newExpense);

        // R - Read (Collection)
        // The userId parameter enforces that a user can only query their own expenses.
        Task<IEnumerable<Expense>> GetExpensesByUserIdAsync(int userId);

        // R - Read (Single)
        // The userId parameter ensures the expense belongs to the requester.
        Task<Expense> GetExpenseByIdAsync(int expenseId, int userId);

        // U - Update
        // Returns true if the update was successful (and authorized).
        Task<bool> UpdateExpenseAsync(int expenseId, int userId, ExpenseUpdateDTO updatedExpense);

        // D - Delete
        // Returns true if the deletion was successful (and authorized).
        Task<bool> DeleteExpenseAsync(int expenseId, int userId);

        // Bonus: Business Logic/Reporting
        Task<decimal> GetTotalExpensesForMonthAsync(int userId, int year, int month);
    }
}