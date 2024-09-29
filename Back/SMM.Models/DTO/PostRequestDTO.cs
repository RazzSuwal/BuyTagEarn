using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMM.Models.DTO
{
    public class PostRequestDTO
    {
        //public int? UserPostId { get; set; }
        public string? UserId { get; set; }
        public string? ProductName { get; set; }
        public string? PostUrl { get; set; }
        public string? ProductType { get; set; }
        public string? BrandName { get; set; }
        public bool? IsTag { get; set; }
        public DateTime? PostedOn { get; set; }
        public bool? IsPaid { get; set; }
    }
}
