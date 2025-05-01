using FTACADEMY_STUDENT_MANAGEMENT_API.Data;
using FTACADEMY_STUDENT_MANAGEMENT_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FTACADEMY_STUDENT_MANAGEMENT_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly FtacademyStudentManagementContext _dbContext;
        public UserController(FtacademyStudentManagementContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [Route("user_profile/{userID:int}")]
        public IActionResult GetUserProfile(int userID)
        { 
            var userProfile = _dbContext.Users.Find(userID);
            return Ok(userProfile);
        }

        [HttpGet("all_user_profiles")]
        public IActionResult GetAllUserProfiles()
        {
            var userProfiles = _dbContext.Users.ToList();
            return Ok(userProfiles);
        }

        [HttpDelete("delete_user/{userID:int}")]
        public async Task<ActionResult<User>> deleteUser(int userID)
        {
            try
            {
                var user = await _dbContext.Users.FindAsync(userID);
                if (user != null)
                    _dbContext.Users.Remove(user);
                else
                    return NotFound($"User with id {userID} not found");

                if (user.Role == "Instructor")
                    return BadRequest("Invalid User Removal Method");

                return Ok("User Deleted Successfuly");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
