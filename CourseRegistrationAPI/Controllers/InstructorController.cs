using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using CourseRegistrationAPI.Data;
using CourseRegistrationAPI.Model;

[Route("api/instructor/courses")]
[ApiController]
[Authorize(Roles = "Instructor")]
public class InstructorCourseController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public InstructorCourseController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/instructor/courses
    [HttpGet]
    public async Task<IActionResult> GetMyCourses()
    {
        var instructorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (instructorId == null) return Unauthorized();

        var courses = await _context.Courses
            .Where(c => c.InstructorId == instructorId)
            .ToListAsync();

        return Ok(courses);
    }

    // PUT: api/instructor/courses/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMyCourse(int id, [FromBody] Course model)
    {
        var instructorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (instructorId == null) return Unauthorized();

        var course = await _context.Courses.FindAsync(id);
        if (course == null) return NotFound("Course not found.");
        if (course.InstructorId != instructorId) return Forbid("You can only update your own courses.");

        course.Title = model.Title;
        course.Description = model.Description;

        _context.Courses.Update(course);
        await _context.SaveChangesAsync();
        return Ok(course);
    }
}
