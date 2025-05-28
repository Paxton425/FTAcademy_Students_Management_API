using FTACADEMY_STUDENT_MANAGEMENT_API.Data;
using FTACADEMY_STUDENT_MANAGEMENT_API.Models;
using FTACADEMY_STUDENT_MANAGEMENT_API.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FTACADEMY_STUDENT_MANAGEMENT_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public StudentsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //Gets all students data
        [HttpGet("all_students_data")]
        public IActionResult GetAllStudents() {
            var students = _dbContext.Students.ToList();
            return Ok(students);
        }

        //Gets a specific students dta
        [HttpGet]
        [Route("student_data/{studentID:int}")]
        public IActionResult GetStudentData(int studentID)
        {
            var student = _dbContext.Students.Find(studentID);
            if (student != null)
                return Ok(student);
            else
                return StatusCode(StatusCodes.Status404NotFound, $"No Student Found With id : {studentID}");
        }

        //Gets a specific student's profile information
        [HttpGet]
        [Route("student_profile/{studentID}")]
        public async Task<ActionResult<object>> GetStudentProfileById(int studentID)
        {
            try
            {
                if (studentID == null)
                    return StatusCode(StatusCodes.Status400BadRequest, "Invalid Student Id");

                IQueryable<Student> query = _dbContext.Students
                    .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Qualification)
                    .Where(s => s.StudentId == studentID); // Ensure Qualification is loaded

                var studentProfile = await query.Select(s => new
                {
                    s.StudentId,
                    s.FirstName,
                    s.LastName,
                    s.StudentNumber,
                    Enrollments = s.Enrollments.Select(e => new
                    {
                        e.QualificationId,
                        Qualification = e.Qualification,
                        e.EnrollmentDate,
                    }).ToList(),
                    s.PhoneNumber,
                    s.Email,
                    s.DateOfBirth,
                    s.EnrollmentStatus,
                    s.ProfileImageUrl,
                })
                .FirstOrDefaultAsync();

                if (studentProfile == null)
                    return NotFound($"No Student With id: {studentID} found");

                return Ok(studentProfile);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        //Gets all student's profile information or filtered by name/student number
        [HttpGet]
        [Route("all_student_profiles")]
        [HttpGet("profiles")]
        public async Task<ActionResult<IEnumerable<object>>> GetStudentProfilesByNameOrIdOrAll(string firstName = null, string studentNumber = null)
        {
            try
            {
                IQueryable<Student> query = _dbContext.Students
                    .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Qualification);

                if (!string.IsNullOrEmpty(firstName))
                {
                    query = query.Where(s => s.FirstName.Contains(firstName));
                }

                if (!string.IsNullOrEmpty(studentNumber))
                {
                    query = query.Where(s => s.StudentNumber.Contains(studentNumber));
                }

                var studentProfiles = await query.Select(s => new
                {
                    s.StudentId,
                    s.FirstName,
                    s.LastName,
                    s.StudentNumber,
                    s.EnrollmentStatus,
                    s.ProfileImageUrl,
                    s.Email,
                    s.DateOfBirth,
                    Enrollments = s.Enrollments.Select(e => new
                    {
                        e.EnrollmentId,
                        e.EnrollmentDate,
                        QualificationId = e.QualificationId,
                        QualificationName = e.Qualification.QualificationName
                    }).ToArray() // Project enrollments into an array
                })
                .ToListAsync();

                if (!studentProfiles.Any())
                {
                    return NotFound();
                }

                return Ok(studentProfiles);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("student_courses/{studentID:int}")]
        public async Task<ActionResult<IEnumerable<object>>> getStudentCourses(int studentID)
        {
            try
            {
                IQueryable<Student> query = _dbContext.Students
                .Include(s => s.Enrollments)
                .ThenInclude(s => s.Qualification);

                var studentCourses = query.Select(s => new {
                    s.StudentId,
                    s.FirstName,
                    s.LastName,
                    s.StudentNumber,
                    Qualifications = s.Enrollments.Select(e => new {
                        e.Qualification.QualificationId,
                        e.Qualification.QualificationName,
                        Courses = e.Qualification.Courses,
                    }).ToList(),
                }).ToList().Where(s => s.StudentId == studentID);

                if (studentCourses.Any())
                    return Ok(studentCourses);

                return NotFound();
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("add_student_data")]
        public IActionResult AddStudent(StudentDTO studentDto)
        {
            try
            {
                var newStudent = new Student
                {
                    FirstName = studentDto.FirstName,
                    LastName = studentDto.LastName,
                    StudentNumber = studentDto.StudentNumber,
                    Email = studentDto.Email,
                    DateOfBirth = studentDto.DateOfBirth,
                    EnrollmentStatus = studentDto.EnrollmentStatus,
                    ProfileImageUrl = studentDto.ProfileImageUrl,
                };

                _dbContext.Add(newStudent);
                _dbContext.SaveChanges();

                return Ok("Student Added, Successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        //Enrolls a student and creates a profile
        [HttpPost]
        [Route("add_student_enrollment")]
        public async Task<ActionResult<Student>> AddStudentEnrollment(StudentDTO studentDto)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            Student savedStudent = null;
            try
            {
                var newStudent = new Student
                {
                    FirstName = studentDto.FirstName,
                    LastName = studentDto.LastName,
                    StudentNumber = studentDto.StudentNumber,
                    DateOfBirth = studentDto.DateOfBirth,
                    Email = studentDto.Email,
                    PhoneNumber = studentDto.PhoneNumber,
                    EnrollmentStatus = "Active",
                    ProfileImageUrl = studentDto.ProfileImageUrl,
                };

                _dbContext.Add(newStudent);
                await _dbContext.SaveChangesAsync();

                savedStudent = await _dbContext.Students
                    .FirstOrDefaultAsync(s => s.StudentNumber == newStudent.StudentNumber);

                if (savedStudent != null && studentDto.Enrollments != null && studentDto.Enrollments.Any())
                {
                    List<Enrollment> enrollments = studentDto.Enrollments
                        .Select(enrollmentDto => new Enrollment
                        {
                            StudentId = savedStudent.StudentId,
                            QualificationId = enrollmentDto.QualificationId,
                            EnrollmentDate = DateTime.Now,
                        }).ToList();

                    // Correctly check for duplicate qualification enrollments for the student BY GROUPING ITEMS WITH SAME QULIFICATION-ID
                    var duplicateQualification = enrollments
                        .GroupBy(e => e.QualificationId)
                        .Any(g => g.Count() > 1);

                    if (duplicateQualification)
                    {
                        _dbContext.Remove(savedStudent);
                        await _dbContext.SaveChangesAsync();
                        await transaction.RollbackAsync();
                        return BadRequest("Duplicate Qualification Enrollments Found for the same student in this request!");
                    }

                    await _dbContext.AddRangeAsync(enrollments);
                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return StatusCode(StatusCodes.Status200OK, "Student Profile Saved Successfully.");
                }
                else if (savedStudent != null && (studentDto.Enrollments == null || !studentDto.Enrollments.Any()))
                {
                    await transaction.CommitAsync();
                    return StatusCode(StatusCodes.Status200OK, "Student Saved Successfully (No Enrollments Provided).");
                }
                else
                {
                    await transaction.RollbackAsync();
                    return StatusCode(StatusCodes.Status400BadRequest, "Something went wrong while saving the student.");
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("update_student_data/{studentID:int}")]
        public async Task<ActionResult<Student>> UpdateStudent(int studentID, StudentDTO UpdatedStudent)
        {
            try
            {
                var studentToUpdate = await _dbContext.Students.FindAsync(studentID);

                if (studentToUpdate is not null)
                {
                    studentToUpdate.FirstName = UpdatedStudent.FirstName;
                    studentToUpdate.LastName = UpdatedStudent.LastName;
                    studentToUpdate.StudentNumber = UpdatedStudent.StudentNumber;
                    studentToUpdate.ProfileImageUrl = UpdatedStudent.ProfileImageUrl;
                    studentToUpdate.Email = UpdatedStudent.Email;
                    studentToUpdate.DateOfBirth = UpdatedStudent.DateOfBirth;
                    studentToUpdate.PhoneNumber = UpdatedStudent.PhoneNumber;

                    _dbContext.SaveChanges();
                    return StatusCode(StatusCodes.Status200OK, studentToUpdate);
                }
                else
                    return NotFound("Student Does not Exist!");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An Error Occureed while Updating Student Profile");
            }
        }

        [HttpDelete]
        [Route("delete_enrollment/{enrollmentID:int}")]
        public IActionResult AddStudentEnrollment(int enrollmentID)
        {
            try
            {
                var enrollment = _dbContext.Enrollments.Find(enrollmentID);

                if (enrollment != null)
                {
                    _dbContext.Remove(enrollment);
                    _dbContext.SaveChanges();

                    return Ok("Student Enrollment Deleted!");
                }
                else
                    return StatusCode(StatusCodes.Status400BadRequest, "Enrollment Found");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }


        [HttpDelete]
        [Route("delete_student_profile/{studentID:int}")]
        public async Task<ActionResult<Student>> DeleteStudent(int studentID) 
        {
            try
            {
                var studentToDelete = await _dbContext.Students.FindAsync(studentID);

                List<Enrollment> studentEnrollments = await _dbContext.Enrollments
                    .Where(s => s.StudentId == studentToDelete.StudentId)
                    .ToListAsync();

                if (studentEnrollments != null)
                    _dbContext.RemoveRange(studentEnrollments);

                if (studentToDelete != null)
                {
                    _dbContext.Remove(studentToDelete);
                    _dbContext.SaveChanges();

                    return StatusCode(StatusCodes.Status200OK, "Student Profile Deleted Successfully!");
                }
                else
                    return NotFound($"Student with id = {studentID} Does not Exist!");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Deleting Student Profile");
            }
        }
    }
}
