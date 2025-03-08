using CourseRegistrationAPI.Data;
using CourseRegistrationAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CourseRegistrationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> usermanager;

        public EnrollmentsController(ApplicationDbContext context, UserManager<ApplicationUser> usermanager)
        {
            _context = context;
            this.usermanager = usermanager;
        }

        [Authorize(Roles = "Student")]
        [HttpGet("my-courses")]

        public async Task<IActionResult> GetMyCourses()
        {
            var studid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var enrollments = await _context.Enrollments
                .Where(e => e.StudnetId == studid)
                .Include(e => e.Course)
                .ToListAsync();
            return Ok(enrollments);
        }


        [Authorize(Roles = "Student")]
        [HttpPost("courseId")]

        public async Task<IActionResult> EnrollInCourses(int courseid)
        {
            var studid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var course = await _context.Courses.FirstAsync(c => c.Id == courseid);
               

            if (course is null)
                return NotFound("Course not found");

            var isenrolled = await _context.Enrollments
                .AnyAsync(e => e.StudnetId == studid && e.CourseId == courseid);
            if (isenrolled)
                return BadRequest("already enrolled");
            ;
            var enrollments = new Enrollment
            {
                StudnetId = studid,
                CourseId = courseid
            };
            _context.Enrollments.Add(enrollments);
            await _context.SaveChangesAsync();
            return Ok(new { mesage = "Successfully enrolled in the course" });
            //return Ok(enrollments);
        }

        [Authorize(Roles = "Student")]
        [HttpDelete("{id}")]

         public async Task<IActionResult> DropCourse(int id)
        {
            var studid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(c => c.StudnetId == studid && c.CourseId == id);

            if (enrollment is null)
                return NotFound("You are not enrolled in this course");

            _context.Enrollments.Remove(enrollment);

            await _context.SaveChangesAsync();

            return Ok(new { message = "Successfully dropped the course" });


        }



    }
}
