using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMM.Data;
using SMM.DataAccessLayer.Services.IServices;
using SMM.Models.DTO;

namespace SMM.Areas.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly AppDbContext _db;
        protected ResponseDTO _response;
        private readonly IEmailSender _emailSender;

        public UsersController(IAuthService authService, AppDbContext db, IEmailSender emailSender)
        {
            _authService = authService;
            _response = new();
            _db = db;
            _emailSender = emailSender;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO model)
        {
            //Email
            var receiver = model.Email;
            var subject = string.Empty;
            var message = string.Empty;
            if (model.Role == "BRAND")
            {
                subject = "Reset Your Password";
                message = $"Hello {model.Name},\n\n" +
                              "We have set a new password for your account. " +
                              "Your new password is: " + model.Password + "\n\n" +
                              "Please change your password from the following URL:\n" +
                              "http://localhost:53430/changePassword\n\n" +
                              "If you did not request this change, please contact support immediately.";

            }
            else
            {
                subject = "Account Creation Successful - Verify Your Email";
                message = $"Dear {model.Name},\n\n" +
                              "You have successfully created your account. " +
                              "Please verify that it was you by clicking the link below:\n" +
                              "<Insert Verification Link Here>\n\n" +
                              "If you did not create this account, please contact support immediately.";


            }
            await _emailSender.SendEmailAsync(receiver, subject, message);

            var errorMessage = await _authService.Register(model);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                _response.Success = false;
                _response.Message = errorMessage;
                return BadRequest(_response);
            }
            return Ok(_response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDTO model)
        {
            var loginResponse = await _authService.Login(model);
            if (loginResponse.User == null)
            {
                _response.Success = false;
                _response.Message = "Username or password is incorrect";
                _response.Result = "";
                return Ok(_response);

            }
            _response.Result = loginResponse;
            return Ok(_response);
        }
        [HttpPost("AssignRole")]
        //[Authorize]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDTO model)
        {
            var assignRoleSuccessful = await _authService.AssignRole(model.Email, model.Role.ToUpper());
            if (!assignRoleSuccessful)
            {
                _response.Success = false;
                _response.Message = "Error encountered";
                return BadRequest(_response);

            }
            return Ok(_response);
        }
        [HttpGet("Dashboard")]
        [Authorize]
        public IActionResult Dashboard()
        {
            var message = "Welcome to the Dashboard!";
            return Ok(new { Message = message });
        }

        [HttpGet("GetUserRole/{email}")]
        [Authorize]
        public async Task<IActionResult> GetUserRole(string email)
        {
            var roleName = await _authService.GetUserRole(email);
            return Ok(roleName);
        }
        //[HttpGet("GetAllBrand")]
        //[Authorize]
        //public async Task<IActionResult> GetAllBrand()
        //{
        //    var result = await _authService.GetAllBrandAsync();
        //    return Ok(result);
        //}

        [HttpPost("ChangePassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDTO model)
        {
            if (model == null || string.IsNullOrEmpty(model.OldPassword) || string.IsNullOrEmpty(model.NewPassword))
            {
                _response.Success = false;
                _response.Message = "Old and new password must be provided.";
                return BadRequest(_response);
            }

            var result = await _authService.ChangePassword(model.Email, model.OldPassword, model.NewPassword);
            if (result == "Password changed successfully!")
            {
                _response.Success = true;
                _response.Message = result;
                return Ok(_response);
            }
            _response.Success = false;
            _response.Message = result;
            return BadRequest(_response);
        }
    }
}

