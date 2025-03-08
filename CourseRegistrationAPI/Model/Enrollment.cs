namespace CourseRegistrationAPI.Model
{
    public class Enrollment
    {
        public int Id { get; set; }

        public string? StudnetId { get; set; }

        public ApplicationUser? Student { get; set; }
        public int CourseId { get; set; }
        public Course? Course { get; set; } //foreign key

       
    }
}
