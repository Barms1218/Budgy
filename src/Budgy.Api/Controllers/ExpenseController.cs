using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Budgy.Application.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class ExpenseController : ControllerBase
{
    private readonly IExpenseService _expenseService;

    public ExpenseController(IExpenseService expenseService)
    {
        _expenseService = expenseService;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<ExpenseDTO>>> GetExpenses()
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdString == null)
        {
            return Unauthorized("Invalid token.");
        }

        var userId = int.TryParse(userIdString, out var id) ? id : 0;


        var expenses = await _expenseService.GetExpensesByUserIdAsync(userId);

        return Ok(expenses);
    }


    [HttpPost]
    [Authorize]
    public async Task<ActionResult<ExpenseDTO>> AddExpense([FromBody] ExpenseCreateDTO dto)
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdString == null)
        {
            return Unauthorized("Invalid token.");
        }

        var userId = int.TryParse(userIdString, out var id) ? id : 0;

        var createdExpense = await _expenseService.CreateExpenseAsync(userId, dto);

        if (createdExpense == null)
        {
            return StatusCode(500, "A problem happened while handling your request.");
        }

        return CreatedAtAction(nameof(GetExpenses), new { userId = userId }, createdExpense);
    }

    [HttpPut("{expenseID}")]
    [Authorize]
    public async Task<ActionResult> UpdateExpense(int expenseID, [FromBody] ExpenseUpdateDTO dto)
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdString == null)
        {
            return Unauthorized("Invalid token.");
        }

        var id = int.TryParse(userIdString, out var idValue) ? idValue : 0;

        if (id <= 0 || expenseID <= 0)
        {
            return BadRequest("Invalid user ID or expense ID.");
        }

        var success = await _expenseService.UpdateExpenseAsync(id, expenseID, dto);

        if (!success)
        {
            return NotFound("Expense not found or could not be updated.");
        }

        return NoContent();
    }

    [HttpDelete("{expenseID}")]
    [Authorize]
    public async Task<ActionResult> DeleteExpense(int expenseID)
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdString == null)
        {
            return Unauthorized("Invalid token.");
        }

        var id = int.TryParse(userIdString, out var idValue) ? idValue : 0;

        if (expenseID <= 0)
        {
            return BadRequest("Invalid expense ID.");
        }

        var success = await _expenseService.DeleteExpenseAsync(id, expenseID);

        if (!success)
        {
            return NotFound("Expense not found.");
        }

        return NoContent();
    }
}