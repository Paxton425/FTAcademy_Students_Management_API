using System;
using System.Collections.Generic;

namespace FTACADEMY_STUDENT_MANAGEMENT_API.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string? Email { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public virtual GoogleAccessToken? GoogleAccessToken { get; set; }

    public virtual Instructor? Instructor { get; set; }
}
