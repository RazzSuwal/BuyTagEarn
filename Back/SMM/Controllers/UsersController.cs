﻿using Microsoft.AspNetCore.Authorization;
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

        public UsersController(IAuthService authService, AppDbContext db)
        {
            _authService = authService;
            _response = new();
            _db = db;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO model)
        {
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

    }
}

