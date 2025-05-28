using FTACADEMY_STUDENT_MANAGEMENT_API.Data;
using FTACADEMY_STUDENT_MANAGEMENT_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FTACADEMY_STUDENT_MANAGEMENT_API.Controllers
{
    public class CourseController : Controller
    {

        private readonly ApplicationDbContext _dbContext;
        public CourseController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("courses")]
        public IActionResult AllCourses()
        {
            var courses = _dbContext.Courses.ToList();
            return Ok(courses);
        }

        [HttpGet("department/qualification_courses")]
        public async Task<ActionResult<IEnumerable<object>>> AllDepartMentCourses()
        {
            try
            {
                var departments = await _dbContext.Departments
                    .Select(d => new
                    {
                        d.DeptId,
                        d.DeptName,
                        d.DeptDescription,
                        Qualifications = d.Qualifications.Select(q => new {
                            q.QualificationId,
                            q.QualificationName,
                            Courses = q.Courses.Select(c => new {
                                c.CourseId,
                                c.CourseName,
                                c.CourseCode
                            })
                        }).ToList()
                    })
                    .ToListAsync();

                return Ok(departments);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
