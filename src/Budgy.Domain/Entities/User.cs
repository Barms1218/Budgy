namespace Budgy.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    }
}