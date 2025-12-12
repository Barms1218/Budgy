using Budgy.Domain.Entities;

namespace Budgy.Application.DTOs
{
    public class UserDTO
{
    public int Id { get; set; }
    public string UserName { get; set; } = String.Empty;
    public ICollection<Expense>? Expenses { get; set; }
}
}