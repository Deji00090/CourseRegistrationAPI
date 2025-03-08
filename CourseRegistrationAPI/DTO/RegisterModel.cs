using System.ComponentModel.DataAnnotations;

namespace CourseRegistrationAPI.DTO
{
    public class RegisterModel
    {
        [Required]
        public string? FullName { get; set; }

        [Required,EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }

        public bool IsInstructor { get; set; }

    }
}
