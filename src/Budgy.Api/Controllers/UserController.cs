using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Budgy.Application.Interfaces;
using Budgy.Application.DTOs;

namespace Budgy.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("profile")]
        public async Task<ActionResult<UserDTO>> GetProfile()
        {
            var userIDString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIDString == null)
            {
                return Unauthorized("Invalid token.");
            }

            var userID = int.TryParse(userIDString, out var id) ? id : 0;

            var user = await _userService.GetUserByIdAsync(userID);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(user);
        }

        [HttpDelete("delete")]
        [Authorize]
        public async Task<ActionResult> DeleteUser()
        {
            var userIDString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIDString == null)
            {
                return Unauthorized("Invalid token.");
            }

            var userID = int.TryParse(userIDString, out var id) ? id : 0;

            var user = await _userService.GetUserByIdAsync(userID);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var success = await _userService.DeleteUserAsync(userID);

            if (!success)
            {
                return NotFound("User not found.");
            }

            return NoContent();
        }
    }
}