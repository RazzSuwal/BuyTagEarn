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
        private readonly IPostService _postService;

        public LocalUserController(IUserService userService, IAuthService authService, IPostService postService)
        {
            _userService = userService;
            _authService = authService;
            _postService = postService;
        }

        [HttpPost("UserPosts")]
        [Authorize]
        public async Task<IActionResult> UserPost([FromForm] PostRequestDTO model)
        {
            var userDetails = _authService.GetLoggedInUserDetails(User) as UserDTO;

            if (userDetails == null || string.IsNullOrEmpty(userDetails.ID))
                return BadRequest("User is missing");

            model.UserId = userDetails.ID;

            if (model.file == null || model.file.Length == 0)
                return BadRequest("No file uploaded.");

            try
            {
                // Fetch product images from the database based on product name
                var productDetails = await _postService.GetProductImageByProductName(model.ProductName);

                if (productDetails is string errorMessage)
                    return NotFound(errorMessage);

                // Load and normalize the uploaded image
                byte[] uploadedImageBytes;
                using (var memoryStream = new MemoryStream())
                {
                    await model.file.CopyToAsync(memoryStream);
                    uploadedImageBytes = memoryStream.ToArray();
                }

                var uploadedVector = GetNormalizedPixelVector(uploadedImageBytes);

                bool isSimilar = false;

                foreach (var data in productDetails)
                {
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), data.ImageUrl.TrimStart('/'));
                    if (!System.IO.File.Exists(imagePath)) continue;

                    byte[] existingImageBytes = await System.IO.File.ReadAllBytesAsync(imagePath);
                    var existingVector = GetNormalizedPixelVector(existingImageBytes);

                    // Calculate similarity
                    double similarity = CalculateCosineSimilarity(uploadedVector, existingVector);

                    if (similarity >= 0.9) // Threshold for similarity
                    {
                        isSimilar = true;
                        break;
                    }
                }

                if (!isSimilar)
                {
                    return BadRequest(new { message = "Uploaded image does not match any existing images." });
                }

                // Save the image and post data
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


        private float[] GetNormalizedPixelVector(byte[] imageBytes)
        {
            using (var ms = new MemoryStream(imageBytes))
            using (var bitmap = new System.Drawing.Bitmap(ms))
            {
                var resized = new System.Drawing.Bitmap(bitmap, new System.Drawing.Size(32, 32));
                var pixelVector = new List<float>();
                for (int y = 0; y < resized.Height; y++)
                {
                    for (int x = 0; x < resized.Width; x++)
                    {
                        var pixel = resized.GetPixel(x, y);
                        float grayValue = (pixel.R + pixel.G + pixel.B) / 3.0f;
                        pixelVector.Add(grayValue / 255.0f);
                    }
                }
                return pixelVector.ToArray();
            }
        }

        private double CalculateCosineSimilarity(float[] vectorA, float[] vectorB)
        {
            if (vectorA.Length != vectorB.Length)
                throw new ArgumentException("Vectors must be of the same length.");

            double dotProduct = 0, normA = 0, normB = 0;

            for (int i = 0; i < vectorA.Length; i++)
            {
                dotProduct += vectorA[i] * vectorB[i];
                normA += Math.Pow(vectorA[i], 2);
                normB += Math.Pow(vectorB[i], 2);
            }

            return dotProduct / (Math.Sqrt(normA) * Math.Sqrt(normB));
        }



        //public async Task<IActionResult> UserPost([FromForm] PostRequestDTO model)
        //{
        //    // Cast or ensure that GetLoggedInUserDetails returns a UserDTO
        //    var userDetails = _authService.GetLoggedInUserDetails(User) as UserDTO;

        //    if (userDetails == null || string.IsNullOrEmpty(userDetails.ID))
        //    {
        //        return BadRequest("User is missing");
        //    }
        //    model.UserId = userDetails.ID;
        //    if (model.file == null || model.file.Length == 0)
        //    {
        //        return BadRequest("No file uploaded.");
        //    }
        //    try
        //    {

        //        var postDetails = await _postService.GetProductImageByProductName(model.ProductName);

        //        if (postDetails is string errorMessage)
        //        {
        //            return NotFound(errorMessage);
        //        }

        //        // Add image data to each post record
        //        foreach (var post in postDetails)
        //        {
        //            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), post.ImageUrl.TrimStart('/'));
        //            if (System.IO.File.Exists(imagePath))
        //            {
        //                byte[] imageBytes = await System.IO.File.ReadAllBytesAsync(imagePath);
        //                post.ImageData = Convert.ToBase64String(imageBytes);  // Add base64 image data to post
        //            }
        //        }

        //        string uploadsFolder = Path.Combine("Uploads", "PostImage");
        //        string filePath = Path.Combine(Directory.GetCurrentDirectory(), uploadsFolder);

        //        if (!Directory.Exists(filePath))
        //        {
        //            Directory.CreateDirectory(filePath);
        //        }

        //        string uniqueFileName = $"{Guid.NewGuid()}_{model.file.FileName}";
        //        string fullPath = Path.Combine(filePath, uniqueFileName);
        //        string imageUrl = $"/{uploadsFolder}/{uniqueFileName}";

        //        using (var stream = new FileStream(fullPath, FileMode.Create))
        //        {
        //            await model.file.CopyToAsync(stream);
        //        }
        //        model.ImageUrl = imageUrl;

        //        var result = await _userService.UserPost(model);
        //        return Ok(new { message = "Post successfully created!" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { Error = ex.Message, StackTrace = ex.StackTrace });
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
