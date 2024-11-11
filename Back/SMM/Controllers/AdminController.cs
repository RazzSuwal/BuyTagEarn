using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMM.DataAccessLayer.Services.IServices;
using SMM.Models.DTO;

namespace SMM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IAuthService _authService;
        public AdminController(IPostService postService, IAuthService authService)
        {
            _postService = postService;
            _authService = authService;
        }
        [HttpGet("GetUserPosts/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetAllUserPosts(string? userId)
        {
            // Validate the Id input
            //if (string.IsNullOrEmpty(userId))
            //{
            //    return BadRequest("UserId cannot be null or empty");
            //}

            try
            {

                var userDetails = _authService.GetLoggedInUserDetails(User) as UserDTO;


                if (userDetails == null || string.IsNullOrEmpty(userDetails.ID))
                {
                    return BadRequest("User is missing");
                }
                var role = await _authService.GetUserRoleById(userDetails.ID);
                string id = userId;
                if (role == "ADMIN")
                {
                    id = null;
                }
                var postDetails = await _postService.GetAllUserPost(id);
                if (postDetails is string errorMessage)
                {
                    return NotFound(errorMessage);
                }
                if (postDetails == null || !((IEnumerable<dynamic>)postDetails).Any())
                {
                    return NotFound("No posts found for the given type");
                }

                return Ok(postDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }


        [HttpPost("AprovedUserPost/{postId}/{IsApproved}")]
        [Authorize]
        public async Task<IActionResult> AprovedUserPost(int postId, int IsApproved)
        {
            try
            {
                var postDetails = await _postService.AprovedUserPost(postId, IsApproved);


                return Ok(new { message = "Approved Successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }
    }
}
