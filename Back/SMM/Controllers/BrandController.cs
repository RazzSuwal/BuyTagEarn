using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMM.DataAccessLayer.Services.IServices;
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
        public async Task<IActionResult> CreateUpdateProduct([FromBody] ProductDTO model)
        {
            var userDetails = _authService.GetLoggedInUserDetails(User) as UserDTO;

            if (userDetails == null || string.IsNullOrEmpty(userDetails.ID))
            {
                return BadRequest("User is missing");
            }

            model.UserId = userDetails.ID;

            var result = await _postService.CreateUpdateProduct(model);
            return Ok(result);
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
    }
}
