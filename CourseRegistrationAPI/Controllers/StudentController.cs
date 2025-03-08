using CourseRegistrationAPI.Data;
using CourseRegistrationAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseRegistrationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> usermanager;

        public StudentController(ApplicationDbContext context, UserManager<ApplicationUser> usermanager)
        {
            _context = context;
            this.usermanager = usermanager;
        }

        [Authorize(Roles = "Student,Instructor")]
        [HttpGet]
        public async Task<IActionResult> Courses()
        {


            var courses = await _context.Courses.Include(c => c.Instructor)
            .ToListAsync();
            return Ok(courses);

        }



        [Authorize(Roles = "Student,Instructor")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourseById(int id)
        {
            var courses = await _context.Courses.Include(c => c.Instructor).FirstOrDefaultAsync(c => c.Id == id);
            if (courses is null)
                return BadRequest("No courses found");
            return Ok(courses);
        }
    }
}
