using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMM.DataAccessLayer.Services.IServices;

namespace SMM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IPostService _postService;
        public AdminController(IPostService postService)
        {
            _postService = postService;
        }
        [HttpGet("GetUserPosts/{type}")]
        [Authorize]
        public async Task<IActionResult> GetAllUserPosts(string type)
        {
            // Validate the Id input
            //if (string.IsNullOrEmpty(userId))
            //{
            //    return BadRequest("UserId cannot be null or empty");
            //}

            try
            {
                var postDetails = await _postService.GetAllUserPost(type);
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
                if (postDetails is string errorMessage)
                {
                    return NotFound(errorMessage);
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
