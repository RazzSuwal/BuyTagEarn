namespace SMM.DataAccessLayer.Services.IServices
{
    public interface IPostService
    {
        Task<dynamic> GetAllUserPost(string? type);
        Task<dynamic> AprovedUserPost(int postId, int IsApproved);
    }
}
