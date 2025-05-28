using System;
using System.Collections.Generic;

namespace FTACADEMY_STUDENT_MANAGEMENT_API.Models;

public partial class Enrollment
{
    public int EnrollmentId { get; set; }

    public int? StudentId { get; set; }

    public int? QualificationId { get; set; }

    public DateTime? EnrollmentDate { get; set; }

    public virtual Qualification? Qualification { get; set; }

    public virtual Student? Student { get; set; }
}
