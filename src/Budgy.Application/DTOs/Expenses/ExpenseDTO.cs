public class ExpenseDTO
{
    public int Id { get; set; }
    public string Category { get; set; } = String.Empty;
    public decimal Amount { get; set; }
    public DateOnly Date { get; set; }
}