using FTACADEMY_STUDENT_MANAGEMENT_API.Data;
using FTACADEMY_STUDENT_MANAGEMENT_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FTACADEMY_STUDENT_MANAGEMENT_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public EnrollmentController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("enrollments_data")]
        public IActionResult GetEnrollments()
        {
            var enrollments = _dbContext.Enrollments.ToList();

            return Ok(enrollments);
        }

        [HttpGet("students_enrollments")]
        public async Task<ActionResult<IEnumerable<object>>> getStudentsEnrollments()
        {
            IQueryable<Enrollment> query =  _dbContext.Enrollments
                .Include(s => s.Student)
                .Include(e => e.Qualification);

            var studentEnrollments = await query.Select(s => new {
                    s.EnrollmentId,
                    StudentFirstName = s.Student.FirstName,
                    StudentLastName = s.Student.LastName,
                    StudentNumber = s.Student.StudentNumber,
                    QualificationName = s.Qualification.QualificationName,
                    s.EnrollmentDate,
                }).ToListAsync();

            if (!studentEnrollments.Any())
                return NotFound();

            return Ok(studentEnrollments);
        }

        [HttpPost]
        [Route("add_enrollment")]
        public IActionResult AddEnrollment(Enrollment enrollmentDto)
        {
            try
            {
                if (_dbContext.Students.Find(enrollmentDto.StudentId) == null)
                    return StatusCode(StatusCodes.Status400BadRequest, $"No Student with Id {enrollmentDto.StudentId} Found");

                var enrollment = new Enrollment
                {
                    StudentId = enrollmentDto.StudentId,
                    QualificationId = enrollmentDto.QualificationId,
                    EnrollmentDate = DateTime.Now,
                };

                _dbContext.Add(enrollment);
                _dbContext.SaveChanges();

                return Ok("Student Enrollment Added, Successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        [HttpDelete]
        [Route("drop_enrollment/{enrollmentID:int}")]
        public IActionResult DeleteEnrollment(int enrollmentID) 
        {
            var enrollmentToDelete = _dbContext.Enrollments.Find(enrollmentID);
            if (enrollmentToDelete != null)
                _dbContext.Enrollments.Remove(enrollmentToDelete);
            else
                return StatusCode(StatusCodes.Status400BadRequest, $"No enrollment found with Id: {enrollmentID}");
            _dbContext.SaveChanges();

            return Ok("Enrollment Deleted Successfully!");
        }
    }
}
