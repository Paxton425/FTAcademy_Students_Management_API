using System;
using System.Collections.Generic;

namespace FTACADEMY_STUDENT_MANAGEMENT_API.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string? CourseName { get; set; }

    public string? CourseCode { get; set; }

    public int? QualificationId { get; set; }

    public int? InstructorId { get; set; }

    public virtual Instructor? Instructor { get; set; }

    public virtual Qualification? Qualification { get; set; }
}
