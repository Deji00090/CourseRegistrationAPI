using Microsoft.AspNetCore.Identity;

namespace CourseRegistrationAPI.DTO
{
    public class UserDTO
    {
        public class UserDto
        {
            public string Id { get; set; }
            public string Email { get; set; }
            public string Fullname { get; set; }
            public bool IsInstructor { get; set; }
        }

        // ... in your controller action ...

      
    }
}
