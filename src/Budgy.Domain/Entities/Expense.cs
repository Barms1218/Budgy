namespace Budgy.Domain.Entities
{
    public class Expense
    {
        public int Id { get; set; }
        public string Category { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }

        public void Update(decimal newAmount, DateTime newDate, string newCategory)
        {
            Amount = newAmount;
            Date = newDate;
            Category = newCategory;
        }

        public bool isAmountValid(decimal amount)
        {
            return amount > 0;
        }

        public bool isDateValid(DateTime date)
        {
            return date <= DateTime.Now;
        }
    }
}