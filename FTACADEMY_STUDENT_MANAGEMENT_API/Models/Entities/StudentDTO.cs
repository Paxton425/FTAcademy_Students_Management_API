using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FTACADEMY_STUDENT_MANAGEMENT_API.Models.Entities
{
    public class StudentDTO
    {
        public int StudentId { get; set; } 

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? StudentNumber { get; set; } = null!;

        public DateOnly? DateOfBirth { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string? EnrollmentStatus { get; set; }

        public string? ProfileImageUrl { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

        public virtual Qualification? Qualification { get; set; }
    }
}
