using System.Security.Claims;
using FTACADEMY_STUDENT_MANAGEMENT_API.Data;
using FTACADEMY_STUDENT_MANAGEMENT_API.Models;
using FTACADEMY_STUDENT_MANAGEMENT_API.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;

namespace FTACADEMY_STUDENT_MANAGEMENT_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;

        private string _clientId;
        private string _clientSecret;

        public UserController(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("user_profile/{userID:int}")]
        public IActionResult GetUserProfile(int userID)
        { 
            var userProfile = _dbContext.Users.Find(userID);
            return Ok(userProfile);
        }

        [HttpGet]
        [Route("get_user_by_email/{email}")]
        public IActionResult GetUserByEmail(string email) {
            try
            {
                if (String.IsNullOrEmpty(email))
                    return NotFound("The email field is reqiured.");
                Console.WriteLine("emails: " + email);
                var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);
                return Ok(user);
            }
            catch (Exception ex) 
            {
                Console.WriteLine("Could not get user by email.... \n"+ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
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
