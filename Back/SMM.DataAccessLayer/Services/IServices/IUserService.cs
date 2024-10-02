using SMM.Models.DTO;

namespace SMM.DataAccessLayer.Services.IServices
{
    public interface IUserService
    {
        Task<string> UserPost(PostRequestDTO postRequestDTO);
        Task<dynamic> GetUserPost(string userId);

    }
}
