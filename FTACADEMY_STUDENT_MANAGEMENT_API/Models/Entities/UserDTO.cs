using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FTACADEMY_STUDENT_MANAGEMENT_API.Models.Entities
{
    public class UserDTO
    {
        public int UserId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Role { get; set; } = null!;
        public string? Email { get; set; }
        public string? GoogleAcessToken { get; set; }
    }
}
