using CourseRegistrationAPI.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CourseRegistrationAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option) : base(option) { }
   
                
        public DbSet<Course>  Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
    }
}
