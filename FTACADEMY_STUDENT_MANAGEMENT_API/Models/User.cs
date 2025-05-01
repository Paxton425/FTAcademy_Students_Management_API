using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FTACADEMY_STUDENT_MANAGEMENT_API.Models;

[Index("Username", Name = "UQ__Users__536C85E491BDB17F", IsUnique = true)]
public partial class User
{
    [Key]
    [Column("UserID")]
    public int UserId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Username { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string Password { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string Role { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string? Email { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? FirstName { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? LastName { get; set; }

    [InverseProperty("User")]
    public virtual Instructor? Instructor { get; set; }
}
