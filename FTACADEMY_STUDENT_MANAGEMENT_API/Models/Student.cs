using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FTACADEMY_STUDENT_MANAGEMENT_API.Models;

[Table("Student")]
[Index("StudentNumber", Name = "UQ_StudentNumber", IsUnique = true)]
[Index("StudentNumber", Name = "UQ_Student_StudentNumber", IsUnique = true)]
public partial class Student
{
    [Key]
    [Column("StudentID")]
    public int StudentId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string LastName { get; set; } = null!;

    [StringLength(8)]
    [Unicode(false)]
    public string? StudentNumber { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Email { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? PhoneNumber { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? EnrollmentStatus { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? ProfileImageUrl { get; set; }

    [InverseProperty("Student")]
    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
