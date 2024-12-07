using Microsoft.AspNetCore.Http;

namespace SMM.Models.DTO
{
    public class ProductDTO
    {
        public int? ProductId { get; set; }
        public string? UserId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductType { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? file { get; set; }
    }
}
