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

        [HttpGet("GetAllProductById")]
        [Authorize]
        public async Task<IActionResult> GetAllProductById()
        {
            try
            {
                var userDetails = _authService.GetLoggedInUserDetails(User) as UserDTO;

                if (userDetails == null || string.IsNullOrEmpty(userDetails.ID))
                {
                    return BadRequest("User is missing");
                }

                var userId = userDetails.ID;
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
    }
}
