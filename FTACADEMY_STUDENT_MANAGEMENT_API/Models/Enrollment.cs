using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FTACADEMY_STUDENT_MANAGEMENT_API.Models;

[Table("Enrollment")]
[Index("StudentId", "QualificationId", Name = "UQ_Enrollment_Student_Qualification", IsUnique = true)]
public partial class Enrollment
{
    [Key]
    [Column("EnrollmentID")]
    public int EnrollmentId { get; set; }

    [Column("StudentID")]
    public int? StudentId { get; set; }

    [Column("QualificationID")]
    public int? QualificationId { get; set; }

    public DateTime? EnrollmentDate { get; set; }

    [ForeignKey("QualificationId")]
    [InverseProperty("Enrollments")]
    public virtual Qualification? Qualification { get; set; }

    [ForeignKey("StudentId")]
    [InverseProperty("Enrollments")]
    public virtual Student? Student { get; set; }
}
