using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FTACADEMY_STUDENT_MANAGEMENT_API.Models;

[Table("Course")]
public partial class Course
{
    [Key]
    [Column("CourseID")]
    public int CourseId { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? CourseName { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? CourseCode { get; set; }

    [Column("QualificationID")]
    public int? QualificationId { get; set; }

    [Column("InstructorID")]
    public int? InstructorId { get; set; }

    [ForeignKey("InstructorId")]
    [InverseProperty("Courses")]
    public virtual Instructor? Instructor { get; set; }

    [ForeignKey("QualificationId")]
    [InverseProperty("Courses")]
    public virtual Qualification? Qualification { get; set; }
}
