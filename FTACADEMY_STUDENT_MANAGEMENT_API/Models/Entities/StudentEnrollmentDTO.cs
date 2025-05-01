namespace FTACADEMY_STUDENT_MANAGEMENT_API.Models.Entities
{
    public class StudentEnrollmentDTO
    {
        public int EnrollmentId { get; set; }

        public int? StudentId { get; set; }

        public string? PhoneNumber { get; set; }

        public DateTime? EnrollmentDate { get; set; }

        public virtual Qualification? Qualification { get; set; }

        public virtual Student? Student { get; set; }
    }
}
