using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SMM.Data;
using SMM.DataAccessLayer.Services.IServices;
using SMM.Models.Domain;
using SMM.Models.DTO;
using System.Security.Claims;

namespace SMM.DataAccessLayer.Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        public AuthService(AppDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IJwtTokenGenerator jwtTokenGenerator)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
            if (user != null)
            {
                if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                {
                    //create new role if it isnot exit
                    _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
                }
                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }

            return false;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDTO.UserName.ToLower());
            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);
            if (user == null || isValid == false)
            {
                return new LoginResponseDTO() { User = null, Token = "", Message = "User NotFound!" };
            }
            //if user was founf, Generate JWT Token
            var token = _jwtTokenGenerator.GenerateToken(user);

            UserDTO userDTO = new()
            {
                Email = user.Email,
                ID = user.Id,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber
            };

            LoginResponseDTO loginResponseDto = new LoginResponseDTO()
            {
                User = userDTO,
                Token = token,
                Message = "Login Sucessful"
            };
            return loginResponseDto;
        }

        public async Task<string> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            ApplicationUser user = new()
            {
                UserName = registrationRequestDTO.Email,
                Email = registrationRequestDTO.Email,
                NormalizedEmail = registrationRequestDTO.Email.ToUpper(),
                Name = registrationRequestDTO.Name,
                PhoneNumber = registrationRequestDTO.PhoneNumber
            };
            try
            {
                var result = await _userManager.CreateAsync(user, registrationRequestDTO.Password);
                if (result.Succeeded)
                {
                    //// Check if any roles were provided from the frontend
                    var roleName = string.Empty;
                    if (registrationRequestDTO.Role == null)
                    {
                        roleName = "User";
                    }
                    else
                    {
                        roleName = registrationRequestDTO.Role;
                    }

                    await _userManager.AddToRoleAsync(user, roleName);

                    var userToReturn = _db.ApplicationUsers.First(u => u.UserName == registrationRequestDTO.Email);

                    UserDTO userDTO = new()
                    {
                        Email = userToReturn.Email,
                        ID = userToReturn.Id,
                        Name = userToReturn.Name,
                        PhoneNumber = userToReturn.PhoneNumber
                    };
                    return "";
                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }
            }
            catch (Exception ex)
            {

            }
            return "Error Encountered";
        }

        public UserDTO GetLoggedInUserDetails(ClaimsPrincipal user)
        {
            // Extract information from the claims
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = user.FindFirst(ClaimTypes.Name)?.Value;
            var email = user.FindFirst(ClaimTypes.Email)?.Value;

            return new UserDTO
            {
                ID = userId,
                Name = userName,
                Email = email
            };
        }
        public async Task<string> GetUserRole(string email)
        {
            var user = await _db.ApplicationUsers.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());

            if (user != null)
            {

                var userRoles = await _userManager.GetRolesAsync(user);

                if (userRoles.Any())
                {
                    return userRoles.First();
                }
            }

            return null;
        }
        public async Task<IEnumerable<RegistrationRequestDTO>> GetAllBrandAsync()
        {
            var usersWithRoles = await _db.ApplicationUsers
                                           .Join(_db.UserRoles,
                                                 user => user.Id,
                                                 userRole => userRole.UserId,
                                                 (user, userRole) => new { user, userRole })
                                           .Join(_db.Roles,
                                                 userWithRole => userWithRole.userRole.RoleId,
                                                 role => role.Id,
                                                 (userWithRole, role) => new { userWithRole.user, role })
                                           .Where(u => u.role.Name.ToLower() == "brand")
                                           .ToListAsync();

            var result = usersWithRoles.Select(u => new RegistrationRequestDTO
            {
                Name = u.user.UserName,
                Email = u.user.Email,
                PhoneNumber = u.user.PhoneNumber,
                Role = u.role.Name,
            }).ToList();

            return result;
        }


    }
}