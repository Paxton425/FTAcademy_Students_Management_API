using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FTACADEMY_STUDENT_MANAGEMENT_API.Models;

[Table("Qualification")]
public partial class Qualification
{
    [Key]
    [Column("QualificationID")]
    public int QualificationId { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string QualificationName { get; set; } = null!;

    [Column(TypeName = "text")]
    public string? Description { get; set; }

    [Column("DeptID")]
    public int? DeptId { get; set; }

    [Column("InstructorID")]
    public int? InstructorId { get; set; }

    [InverseProperty("Qualification")]
    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    [ForeignKey("DeptId")]
    [InverseProperty("Qualifications")]
    public virtual Department? Dept { get; set; }

    [InverseProperty("Qualification")]
    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    [ForeignKey("InstructorId")]
    [InverseProperty("Qualifications")]
    public virtual Instructor? Instructor { get; set; }
}
