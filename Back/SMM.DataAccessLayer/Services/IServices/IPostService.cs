using SMM.Models.DTO;

namespace SMM.DataAccessLayer.Services.IServices
{
    public interface IPostService
    {
        Task<dynamic> GetAllUserPost(string? type);
        Task<dynamic> AprovedUserPost(int postId, int IsApproved);

        #region ProductCURD
        Task<string> CreateUpdateProduct(ProductDTO productDTO);
        Task<dynamic> GetAllProductById(string? UserId);
        #endregion
        Task<dynamic> GetAllBrand();
    }
}
