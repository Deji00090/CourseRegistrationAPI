using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CourseRegistrationAPI.Model;
using CourseRegistrationAPI.DTO;
using Microsoft.EntityFrameworkCore;
using CourseRegistrationAPI.Data;
using static CourseRegistrationAPI.DTO.UserDTO;

[Route("api/admin")]
[ApiController]
//[Authorize(Roles = "Admin")] // Restrict access to Admins
public class AdminController() : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole>  _roleManager;
    private readonly ApplicationDbContext context;
    public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,ApplicationDbContext context) : this()
    {
        this._userManager = userManager;
        this._roleManager = roleManager;
        this.context = context;
    }

    
    // GET: api/admin/users
    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers()
    {
        //var user = await _userManager.Users.e
        var users = await _userManager.Users
            .Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email ?? "No Email", // Handle nulls
                Fullname = u.Fullname ?? "No Name", // Handle nulls
                //IsInstructor = u.IsInstructor,
            })
            .ToListAsync();

        if (users is null)
        {
            return NotFound("No users found.");
        }
        return Ok(users);
    }

    // POST: api/admin/assign-role
    [HttpPost("assign-role")]
    public async Task<IActionResult> AssignRole([FromBody] RoleAssignmentModel model)
    {
        var user = await _userManager.FindByIdAsync(model.UserId);
        if (user == null) return NotFound("User not found.");

        var roleExists = await _roleManager.RoleExistsAsync(model.Role);
        if (!roleExists) return BadRequest("Invalid role.");

        await _userManager.AddToRoleAsync(user, model.Role);
        await context.SaveChangesAsync();
        return Ok($"Role {model.Role} assigned to {user.Email}.");
    }


    // POST: api/admin/deactivate-user/{userId}
    [HttpPost("deactivate-user/{userId}")]
    public async Task<IActionResult> DeactivateUser(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound("User not found.");

        user.LockoutEnd = DateTime.MaxValue;
        await _userManager.UpdateAsync(user);
        return Ok($"User {user.Email} has been deactivated.");
    }
}



