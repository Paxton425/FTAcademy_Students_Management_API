using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;

namespace FTACADEMY_STUDENT_MANAGEMENT_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAntiforgery _antiforgery;

        public AuthController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        //Handles signing in with google
        [HttpGet("google/signin")]
        [AllowAnonymous]
        public IActionResult GoogleLogin(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl))
            {
                return BadRequest("The 'returnUrl' parameter must be provided.");
            }

            var properties = new AuthenticationProperties { RedirectUri = returnUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("checkauth")]
        public IActionResult CheckAuthentication()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return Ok(new { isAuthenticated = true, userName = User.Identity.Name });
            }
            else
            {
                return Ok(new { isAuthenticated = false });
            }
        }

        [HttpPost("logout")] //  Define the specific route for logout
        [ValidateAntiForgeryToken]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)] //  Require authentication
        public async Task<IActionResult> Logout()
        {
            try
            {
                // Sign out the user.  You might need to adjust the scheme, especially if you have multiple.
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                //  Log the successful logout
                //_logger.LogInformation("User logged out successfully.");
                return Ok(new { message = "Logged out" });
            }
            catch (Exception ex)
            {
                //  Log the error
                //_logger.LogError(ex, "Error during logout");
                return StatusCode(500, new { error = "Logout failed", message = ex.Message });
            }
        }

        [HttpGet("logoutconfirmation")]
        public IActionResult GetLogoutConfirmation()
        {
            var tokens = _antiforgery.GetAndStoreTokens(HttpContext);
            return Ok(new
            {
                token = tokens.RequestToken,
                tokenName = tokens.FormFieldName
            });
        }


        [HttpGet("profile")]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)] //  Require authentication, and specify the scheme
        [ValidateAntiForgeryToken] 
        public IActionResult GetProfile()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var name = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Name)?.Value;
                var email = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;
                var googleId = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                // Consider logging the profile retrieval
                //_logger.LogInformation($"Profile retrieved for user: {email}");

                return Ok(new { Name = name, Email = email, GoogleId = googleId });
            }

            // Log unauthorized access attempt.
            //_logger.LogWarning("Unauthorized attempt to access profile.");
            return Unauthorized(new { error = "Unauthorized", message = "User not authenticated." }); // Return a JSON response
        }
    }
}
