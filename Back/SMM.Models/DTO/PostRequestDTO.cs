using Microsoft.AspNetCore.Http;

namespace SMM.Models.DTO
{
    public class PostRequestDTO
    {
        //public int? UserPostId { get; set; }
        public string? UserId { get; set; }
        public int? ProductName { get; set; }
        public string? PostUrl { get; set; }
        public string? BrandName { get; set; }
        public bool? IsTag { get; set; }
        public DateTime? PostedOn { get; set; }
        public bool? IsPaid { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? file { get; set; }


    }
}
