namespace SMM.Models.DTO
{
    public class PaymentDTO
    {
        public int? RequestId { get; set; }
        public string? UserId { get; set; }
        public string? PaidByUserId { get; set; } = null;
        public int? UserPostId { get; set; }
        public string? MobileNo { get; set; }
        public int? IsPaid { get; set; } = null!;
        public string? Amount { get; set; }

    }
}
