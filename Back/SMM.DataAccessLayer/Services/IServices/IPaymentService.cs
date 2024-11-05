using SMM.Models.DTO;

namespace SMM.DataAccessLayer.Services.IServices
{
    public interface IPaymentService
    {
        Task<string> PaymentRequest(PaymentDTO paymentRequestDTO);
        Task<dynamic> GetAllPaymentRequest();
        Task<dynamic> PaymentById(int requestId);
    }
}
