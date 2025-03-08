using CourseRegistrationAPI.Data;
using CourseRegistrationAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseRegistrationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class AdminCourseController : ControllerBase
    {
        
        private readonly ApplicationDbContext context;

        public AdminCourseController(ApplicationDbContext context)
        {
            
            this.context = context;
        }

        [HttpGet("courses")]
        public async Task<IActionResult> GetAllCourses()
        {
            var courses = await context.Courses.Include(c => c.Instructor)
            .ToListAsync();
            return Ok(courses);
        }

        [HttpPost]
        public IActionResult CreateCourses(Course model)
        {
            if (string.IsNullOrEmpty(model.Title) || string.IsNullOrEmpty(model.Description))
                return BadRequest("Title and decription required");

            var course = new Course
            {
                Title = model.Title,
                Description = model.Description,
                InstructorId = model.InstructorId
            };

            context.Courses.Add(course);
            context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAllCourses), new { id = course.Id }, course);
        }


        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateCourse(int id, Course model)
        {
            var course = await context.Courses.FindAsync(id);
            if (course is null)
                return NotFound("Course not found");

           
            course.Title = model.Title;
            course.Description = model.Description;
            course.InstructorId = model.InstructorId;


            await context.SaveChangesAsync();
            return NoContent();
        }

           

      

       

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await context.Courses.FindAsync(id);
            if (course is null) return NotFound("Course not found");

            context.Courses.Remove(course);
            await context.SaveChangesAsync();
            return Ok($"Course '{course.Title}' deleted.");

        }

      

    }
}

