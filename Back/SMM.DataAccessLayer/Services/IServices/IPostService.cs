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
        Task<dynamic> DeleteProductById(int productId);
        Task<dynamic> GetProductImageByProductName(int ProductName);
        #endregion
        Task<dynamic> GetAllBrand();
        Task<dynamic> AprovedBrandProduct(int productId, int IsApproved);
    }
}
