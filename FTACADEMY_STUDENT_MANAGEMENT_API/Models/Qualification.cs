using System;
using System.Collections.Generic;

namespace FTACADEMY_STUDENT_MANAGEMENT_API.Models;

public partial class Qualification
{
    public int QualificationId { get; set; }

    public string QualificationName { get; set; } = null!;

    public string? Description { get; set; }

    public int? DeptId { get; set; }

    public int? InstructorId { get; set; }

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    public virtual Department? Dept { get; set; }

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public virtual Instructor? Instructor { get; set; }
}
