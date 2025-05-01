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
        private readonly FtacademyStudentManagementContext _dbContext;
        public InstructorController(FtacademyStudentManagementContext dbContext)
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
                FirstName = s.User.FirstName,
                LastName = s.User.LastName,
                s.StaffNumber,
                Department = s.Dept.DepartmentName,
                Courses = s.Courses.Select(c => new { 
                        c.CourseName,
                    })
            }).ToListAsync();

            if (!instructorsProfiles.Any())
                return NotFound();

            return Ok(instructorsProfiles);
                
        }

        [HttpGet("instructor_profile/{id:int}")]
        public IActionResult getInstructor(int id)
        {
            var instructor = _dbContext.Instructors.Find(id);
            if (instructor != null)
            {
                return Ok(instructor);
            }
            else
                return NotFound();
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
