using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FTACADEMY_STUDENT_MANAGEMENT_API.Models;

[Table("Department")]
public partial class Department
{
    [Key]
    [Column("DepartmentID")]
    public int DepartmentId { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string DepartmentName { get; set; } = null!;

    [Column(TypeName = "text")]
    public string? Description { get; set; }

    [InverseProperty("Dept")]
    public virtual ICollection<Instructor> Instructors { get; set; } = new List<Instructor>();

    [InverseProperty("Dept")]
    public virtual ICollection<Qualification> Qualifications { get; set; } = new List<Qualification>();
}
