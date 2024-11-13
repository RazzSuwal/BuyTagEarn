using SMM.Models.DTO;

namespace SMM.DataAccessLayer.Services.IServices
{
    public interface IPaymentService
    {
        Task<string> PaymentRequest(PaymentDTO paymentRequestDTO);
        Task<dynamic> GetAllPaymentRequest();
        Task<dynamic> PaymentById(int requestId);
        Task<dynamic> UpdatePaymentRequestImageUrl(int requestId, string imageUrl);
        Task<dynamic> GetAllPaidById(string? userId);
    }
}
