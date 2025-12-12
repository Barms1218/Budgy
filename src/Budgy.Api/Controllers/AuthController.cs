using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Budgy.Application.Interfaces;
using Budgy.Application.DTOs;

namespace Budgy.Api.Controllers
{
[Controller]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IJwtService _jwtService;

    private readonly IUserService _userService;

    public AuthController(IJwtService jwtService, IUserService userService)
    {
        _jwtService = jwtService;
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDTO>> Register([FromBody] UserRegisterDTO dto)
    {
        var createdUser = await _userService.RegisterAsync(dto);

        if (createdUser == null)
        {
            return Conflict("Username already taken.");
        }

        return Ok(createdUser);
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] UserLoginDTO dto)
    {
        var user = await _jwtService.Login(dto);

        if (user == null)
        {
            return Unauthorized("Invalid username or password.");
        }

        return Ok(new { User = user }); // Return token and user info
    }

    [HttpGet("validate")]
    [Authorize]
    public ActionResult ValidateToken()
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdString == null)
        {
            return Unauthorized("Invalid token.");
        }

        return Ok("Token is valid.");
    }

    [HttpPut("update_password")]
    [Authorize]
    public async Task<ActionResult> UpdatePassword([FromQuery] string newPassword)
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdString == null)
        {
            return Unauthorized("Invalid token.");
        }

        var userId = int.TryParse(userIdString, out var id) ? id : 0;

        var success = await _userService.UpdatePasswordAsync(userId, newPassword);

        if (!success)
        {
            return NotFound("User not found or password doesn't meet requirements.");
        }

        return NoContent();
    }
    
}
}