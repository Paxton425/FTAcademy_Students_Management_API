using FTACADEMY_STUDENT_MANAGEMENT_API.Data;
using FTACADEMY_STUDENT_MANAGEMENT_API.Models;
using FTACADEMY_STUDENT_MANAGEMENT_API.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FTACADEMY_STUDENT_MANAGEMENT_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public InstructorController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("instructors_data")]
        public IActionResult getAllInstructors()
        {
            var instructors = _dbContext.Instructors.ToList();
            return Ok(instructors);
        }

        [HttpGet("instructors_profiles")]
        public async Task<ActionResult<IEnumerable<object>>> getInstructorsProfiles()
        {
            IQueryable<Instructor> query = _dbContext.Instructors
                .Include(d => d.Dept)
                .Include(u => u.User)
                .Include(c => c.Courses);

            var instructorsProfiles = await query.Select(s => new
            {
                s.InstructorId,
                User = s.User,
                s.StaffNumber,
                Dept = s.Dept,
                Courses = s.Courses.Select(c => new { 
                        c.CourseName,
                    })
            }).ToListAsync();

            if (!instructorsProfiles.Any())
                return NotFound();

            return Ok(instructorsProfiles);
                
        }

        [HttpGet("instructor_profile/{instructorId:int}")]
        public async Task<ActionResult<Instructor>> getInstructor(int instructorId)
        {
            try
            {
                IQueryable<Instructor> query = _dbContext.Instructors
                .Include(u => u.User)
                .Include(q => q.Qualifications)
                .ThenInclude(d => d.Dept)
                .Include(c => c.Courses).Where(i => i.InstructorId == instructorId);

                var instructorsProfile = await query.Select(i => new
                {
                    i.InstructorId,
                    User = i.User,
                    i.StaffNumber,
                    Dept = i.Dept,
                    Qualifications = i.Qualifications.Select(q => new { 
                        q.QualificationId,
                        q.QualificationName,
                        Dept = q.Dept
                    }),
                    Courses = i.Courses.Select(c => new {
                        c.CourseName,
                        c.CourseCode,
                    })
                }).FirstOrDefaultAsync();

                if (instructorsProfile != null)
                {
                    return Ok(instructorsProfile);
                }
                else
                    return NotFound();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("add_instructor")]
        public IActionResult AddInstructor(InstructorDTO instructorDto)
        {
            try
            {
                var newInstructor = new Instructor
                {
                    DeptId = instructorDto.DeptId,
                    StaffNumber = instructorDto.StaffNumber,
                };

                _dbContext.Add(newInstructor);
                _dbContext.SaveChanges();

                return Ok(newInstructor);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("update_instructor/{id:int}")]
        public IActionResult UpdateInstructor(InstructorDTO instructorDto)
        {
            try
            {
                var newInstructor = new Instructor
                {
                    InstructorId = instructorDto.InstructorId,
                    DeptId = instructorDto.DeptId,
                    StaffNumber= instructorDto.StaffNumber,
                };

                _dbContext.Add(newInstructor);
                _dbContext.SaveChanges();

                return Ok(newInstructor);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("delete_instructor/{id:int}")]
        public async Task<ActionResult<Instructor>> DeleteInstructor(int id)
        {
            try
            {
                var instructorToDelete = await _dbContext.Instructors.FindAsync(id);

                if (instructorToDelete != null)
                {
                    _dbContext.Remove(instructorToDelete);
                    _dbContext.SaveChanges();
                    return StatusCode(StatusCodes.Status200OK, "Instructor Deleted Successfully!");
                }
                else
                    return NotFound($"Instructor with id = {id} Does not Exist!");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Deleting Instructor Profile");
            }
        }
    }
}
