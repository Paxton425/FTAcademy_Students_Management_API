namespace FTACADEMY_STUDENT_MANAGEMENT_API.Models.Entities
{
    public class InstructorDTO
    {
        public int InstructorId { get; set; }

        public int? UserId { get; set; }

        public int? DeptId { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? StaffNumber { get; set; }

        public virtual Department? Dept { get; set; }

        public virtual ICollection<Qualification> Qualifications { get; set; } = new List<Qualification>();

        public virtual User? User { get; set; }
    }
}
