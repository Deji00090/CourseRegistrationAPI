using Microsoft.AspNetCore.Identity;

namespace CourseRegistrationAPI.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string? Fullname { get; set; }

        public bool? IsInstructor { get; set; } // True = instructor , false = student
    }
}
