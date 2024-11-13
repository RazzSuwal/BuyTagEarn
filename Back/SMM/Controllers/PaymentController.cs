using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMM.DataAccessLayer.Services.IServices;
using SMM.Models.DTO;

namespace SMM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IAuthService _authService;

        public PaymentController(IPaymentService paymentService, IAuthService authService)
        {
            _paymentService = paymentService;
            _authService = authService;
        }

        [HttpPost("PaymentRequest")]
        [Authorize]
        public async Task<IActionResult> PaymentRequest([FromBody] PaymentDTO model)
        {
            // Cast or ensure that GetLoggedInUserDetails returns a UserDTO
            var userDetails = _authService.GetLoggedInUserDetails(User) as UserDTO;

            if (userDetails == null || string.IsNullOrEmpty(userDetails.ID))
            {
                return BadRequest("User is missing");
            }

            model.UserId = userDetails.ID;

            var result = await _paymentService.PaymentRequest(model);
            return Ok(result);
        }

        [HttpGet("GetAllPaymentRequest")]
        [Authorize]
        public async Task<IActionResult> GetAllPaymentRequest()
        {

            try
            {
                var data = await _paymentService.GetAllPaymentRequest();
                if (data is string errorMessage)
                {
                    return NotFound(errorMessage);
                }
                if (data == null || !((IEnumerable<dynamic>)data).Any())
                {
                    return NotFound("No data found !");
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpPut("PaymentById/{requestId}")]
        [Authorize]
        public async Task<IActionResult> PaymentById(int requestId)
        {
            try
            {
                var details = await _paymentService.PaymentById(requestId);


                return Ok(new { message = "Payment Successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpPut("UploadVoucher")]
        public async Task<IActionResult> UploadVoucher(int requestId, [FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            try
            {
                string uploadsFolder = Path.Combine("Uploads", "PaymentVouchers");
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), uploadsFolder);

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                string uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
                string fullPath = Path.Combine(filePath, uniqueFileName);
                string imageUrl = $"/{uploadsFolder}/{uniqueFileName}";

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                var result = await _paymentService.UpdatePaymentRequestImageUrl(requestId, imageUrl);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message, StackTrace = ex.StackTrace });
            }
        }

        [HttpGet("GetAllPaidById")]
        [Authorize]
        public async Task<IActionResult> GetAllPaidById()
        {

            try
            {
                var userDetails = _authService.GetLoggedInUserDetails(User) as UserDTO;

                if (userDetails == null || string.IsNullOrEmpty(userDetails.ID))
                {
                    return BadRequest("User is missing");
                }

                var data = await _paymentService.GetAllPaidById(userDetails.ID);
                if (data is string errorMessage)
                {
                    return NotFound(errorMessage);
                }
                if (data == null || !((IEnumerable<dynamic>)data).Any())
                {
                    return NotFound("No data found !");
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }
    }
}
