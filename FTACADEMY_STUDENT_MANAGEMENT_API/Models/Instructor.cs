using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FTACADEMY_STUDENT_MANAGEMENT_API.Models;

[Table("Instructor")]
[Index("StaffNumber", Name = "UQ_StaffNumber", IsUnique = true)]
[Index("UserId", Name = "UQ__Instruct__1788CCAD0643A0A0", IsUnique = true)]
public partial class Instructor
{
    [Key]
    [Column("InstructorID")]
    public int InstructorId { get; set; }

    [Column("UserID")]
    public int? UserId { get; set; }

    [Column("DeptID")]
    public int? DeptId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? StaffNumber { get; set; }

    [InverseProperty("Instructor")]
    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    [ForeignKey("DeptId")]
    [InverseProperty("Instructors")]
    public virtual Department? Dept { get; set; }

    [InverseProperty("Instructor")]
    public virtual ICollection<Qualification> Qualifications { get; set; } = new List<Qualification>();

    [ForeignKey("UserId")]
    [InverseProperty("Instructor")]
    public virtual User? User { get; set; }
}
