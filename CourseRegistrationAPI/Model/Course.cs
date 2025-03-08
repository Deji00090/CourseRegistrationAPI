using System.ComponentModel.DataAnnotations;

namespace CourseRegistrationAPI.Model
{
    public class Course
    {
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Description { get; set; }

        public string? InstructorId { get; set; } = string.Empty; //foreign key

        public ApplicationUser? Instructor { get; set; }
    }
}
