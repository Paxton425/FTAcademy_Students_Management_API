using FTACADEMY_STUDENT_MANAGEMENT_API.Models;
using Microsoft.EntityFrameworkCore;

namespace FTACADEMY_STUDENT_MANAGEMENT_API.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {
        }

        protected ApplicationDBContext()
        {
        }

        DbSet<Student> Students { get; set; }
    }
}
