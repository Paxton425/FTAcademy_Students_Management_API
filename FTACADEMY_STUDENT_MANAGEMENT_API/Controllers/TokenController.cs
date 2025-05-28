using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Antiforgery;
using FTACADEMY_STUDENT_MANAGEMENT_API.Data;
using Microsoft.EntityFrameworkCore;
using FTACADEMY_STUDENT_MANAGEMENT_API.Models;
using FTACADEMY_STUDENT_MANAGEMENT_API.Models.Entities;
using Azure.Core;

namespace FTACADEMY_STUDENT_MANAGEMENT_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IAntiforgery _antiforgery;
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public TokenController(IAntiforgery antiforgery, 
                              ApplicationDbContext dbContext,
                              IConfiguration configuration)
        {
            _antiforgery = antiforgery;
            _dbContext = dbContext;
            _configuration = configuration;
        }

        [HttpGet("getAllAccessTokens")]
        public IActionResult getAllAccessTokens()
        {
            var accessTokens = _dbContext.GoogleAccessTokens.ToList();
            if (accessTokens.Any())
            {
                return Ok(accessTokens);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("getAccessToken/{userId:int}")]
        public IActionResult GetAccessToken(int userId)
        {
            try
            {
                var accessToken = _dbContext.GoogleAccessTokens.FirstOrDefault(t => t.UserId == userId);
                if (accessToken == null)
                    return NotFound();

                return Ok(accessToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting token ====================" + ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, "Could Not Retrieve Token");
            }
        }

        [HttpPost("storeAccessToken")]
        public async Task<ActionResult<GoogleTokenDTO>> StoreAccessToken(GoogleTokenDTO tokenDTO) {
            try
            {
                var existingToken = _dbContext.GoogleAccessTokens.FirstOrDefault(t => t.UserId == tokenDTO.UserId);
                if(existingToken != null)
                {
                    Console.WriteLine("Existing found");
                    existingToken.AccessToken = tokenDTO.AccessToken;
                    existingToken.RefreshToken = tokenDTO.RefreshToken;
                    existingToken.AccessTokenProvider = tokenDTO.AccessTokenProvider;
                    existingToken.LastUpdate = tokenDTO.LastUpdate;
                    existingToken.TokenExpiry = tokenDTO.TokenExpiry;
                }
                else
                {
                    var newToken = new GoogleAccessToken
                    {
                        UserId = tokenDTO.UserId,
                        AccessToken = tokenDTO.AccessToken,
                        RefreshToken = tokenDTO.RefreshToken,
                        AccessTokenProvider = tokenDTO.AccessTokenProvider,
                        LastUpdate = tokenDTO.LastUpdate,
                        TokenExpiry = tokenDTO.TokenExpiry,
                    };

                    _dbContext.GoogleAccessTokens.Add(newToken);
                }
               await _dbContext.SaveChangesAsync();

                return Ok("Token Saved Successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
