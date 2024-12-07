using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using SMM.DataAccessLayer.Services.IServices;
using SMM.DataAccessLayer.Services.Services;
using SMM.Models.DTO;

namespace SMM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IAuthService _authService;

        public BrandController(IPostService postService, IAuthService authService)
        {
            _postService = postService;
            _authService = authService;
        }


        [HttpPost("CreateUpdateProduct")]
        [Authorize]
        public async Task<IActionResult> CreateUpdateProduct([FromForm] ProductDTO model)
        {
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
                string uploadsFolder = Path.Combine("Uploads", "ProductImage");
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

                var result = await _postService.CreateUpdateProduct(model);
                return Ok(new { message = "Product successfully created!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message, StackTrace = ex.StackTrace });
            }

        }

        [HttpGet("GetAllProductById/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetAllProductById(string? userId)
        {
            try
            {
                var products = await _postService.GetAllProductById(userId);
                if (products is string errorMessage)
                {
                    return NotFound(errorMessage);
                }

                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }


        [HttpGet("GetAllBrand")]
        [Authorize]
        public async Task<IActionResult> GetAllBrand()
        {
            var roleName = await _postService.GetAllBrand();
            return Ok(roleName);
        }

        [HttpPut("AprovedBrandProduct/{productId}/{IsApproved}")]
        [Authorize]
        public async Task<IActionResult> AprovedBrandProduct(int productId, int IsApproved)
        {
            try
            {
                var productDetails = await _postService.AprovedBrandProduct(productId, IsApproved);


                return Ok(new { message = "Approved Successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }
        
        [HttpDelete("DeleteProductById/{productId}")]
        [Authorize]
        public async Task<IActionResult> DeleteProductById(int productId)
        {
            try
            {
                var productDetails = await _postService.DeleteProductById(productId);


                return Ok(new { message = "Delete Successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }
    }
}
