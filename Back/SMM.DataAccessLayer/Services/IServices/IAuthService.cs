using SMM.Models.DTO;
using System.Security.Claims;

namespace SMM.DataAccessLayer.Services.IServices
{
    public interface IAuthService
    {
        Task<string> Register(RegistrationRequestDTO registrationRequestDTO);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<bool> AssignRole(string email, string roleName);
        UserDTO GetLoggedInUserDetails(ClaimsPrincipal user);
        Task<string> GetUserRole(string email);
        Task<IEnumerable<RegistrationRequestDTO>> GetAllBrandAsync();
        Task<string> GetUserRoleById(string userId);
        Task<string> ChangePassword(string email, string oldPassword, string newPassword);
    }
}
