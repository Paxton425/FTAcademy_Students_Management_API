using System;
using System.Collections.Generic;

namespace FTACADEMY_STUDENT_MANAGEMENT_API.Models;

public partial class Instructor
{
    public int InstructorId { get; set; }

    public int? UserId { get; set; }

    public int? DeptId { get; set; }

    public string? StaffNumber { get; set; }

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    public virtual Department? Dept { get; set; }

    public virtual ICollection<Qualification> Qualifications { get; set; } = new List<Qualification>();

    public virtual User? User { get; set; }
}
