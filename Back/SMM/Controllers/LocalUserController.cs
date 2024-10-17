using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMM.DataAccessLayer.Services.IServices;
using SMM.DataAccessLayer.Services.Services;
using SMM.Models.DTO;
using System.Security.Claims;

namespace SMM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocalUserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public LocalUserController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [HttpPost("UserPosts")]
        [Authorize]
        public async Task<IActionResult> UserPost([FromBody] PostRequestDTO model)
        {
            // Cast or ensure that GetLoggedInUserDetails returns a UserDTO
            var userDetails = _authService.GetLoggedInUserDetails(User) as UserDTO;

            if (userDetails == null || string.IsNullOrEmpty(userDetails.ID))
            {
                return BadRequest("User is missing");
            }

            model.UserId = userDetails.ID;

            var result = await _userService.UserPost(model);
            return Ok(result);
        }

        [HttpGet("GetUserPosts/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetUserPosts(string userId)
        {
            // Validate the Id input
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("UserId cannot be null or empty");
            }

            try
            {
                var postDetails = await _userService.GetUserPost(userId);
                if (postDetails is string errorMessage)
                {
                    return NotFound(errorMessage); 
                }
                if (postDetails == null || !((IEnumerable<dynamic>)postDetails).Any())
                {
                    return NotFound("No posts found for the given UserId");
                }

                return Ok(postDetails); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

    }
}
