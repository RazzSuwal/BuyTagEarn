using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMM.DataAccessLayer.Services.IServices;
using SMM.Models.DTO;

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
        public async Task<IActionResult> UserPost([FromForm] PostRequestDTO model)
        {
            // Cast or ensure that GetLoggedInUserDetails returns a UserDTO
            var userDetails = _authService.GetLoggedInUserDetails(User) as UserDTO;

            if (userDetails == null || string.IsNullOrEmpty(userDetails.ID))
            {
                return BadRequest("User is missing");
            }
            model.UserId = userDetails.ID;
            if (model.file == null || model.file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }
            try
            {
                string uploadsFolder = Path.Combine("Uploads", "PostImage");
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), uploadsFolder);

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                string uniqueFileName = $"{Guid.NewGuid()}_{model.file.FileName}";
                string fullPath = Path.Combine(filePath, uniqueFileName);
                string imageUrl = $"/{uploadsFolder}/{uniqueFileName}";

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await model.file.CopyToAsync(stream);
                }
                model.ImageUrl = imageUrl;

                var result = await _userService.UserPost(model);
                return Ok(new { message = "Post successfully created!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message, StackTrace = ex.StackTrace });
            }
        }

        //[HttpGet("GetUserPosts/{userId}")]
        //[Authorize]
        //public async Task<IActionResult> GetUserPosts(string userId)
        //{
        //    // Validate the Id input
        //    if (string.IsNullOrEmpty(userId))
        //    {
        //        return BadRequest("UserId cannot be null or empty");
        //    }

        //    try
        //    {
        //        var postDetails = await _userService.GetUserPost(userId);
        //        if (postDetails is string errorMessage)
        //        {
        //            return NotFound(errorMessage);
        //        }
        //        if (postDetails == null || !((IEnumerable<dynamic>)postDetails).Any())
        //        {
        //            return NotFound("No posts found for the given UserId");
        //        }

        //        return Ok(postDetails);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { Error = ex.Message });
        //    }
        //}

        [HttpGet("GetUserPosts/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetUserPosts(string userId)
        {
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

                // Add image data to each post record
                foreach (var post in postDetails)
                {
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), post.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        byte[] imageBytes = await System.IO.File.ReadAllBytesAsync(imagePath);
                        post.ImageData = Convert.ToBase64String(imageBytes);  // Add base64 image data to post
                    }
                }

                return Ok(postDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }


        [HttpGet("GetUserPostsDetails/{userPostId}")]
        [Authorize]
        public async Task<IActionResult> GetUserPostsDetails(int? userPostId)
        {

            try
            {
                var postDetails = await _userService.GetUserPostsDetails(userPostId);
                if (postDetails is string errorMessage)
                {
                    return NotFound(errorMessage);
                }
                if (postDetails == null)
                {
                    return NotFound("No posts found for the given userPostId");
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
