namespace FTACADEMY_STUDENT_MANAGEMENT_API.Models.Entities
{
    public class UserProfileDTO
    {
        public int UserId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string Username { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string? Email { get; set; }

        public string profileImageUrl { get; set; }
        public string? GoogleAcessToken { get; set; }
    }
}
