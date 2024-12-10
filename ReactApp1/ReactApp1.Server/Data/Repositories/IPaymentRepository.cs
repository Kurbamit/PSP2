using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Data.Repositories
{
    public interface IPaymentRepository
    {
        Task<PaginatedResult<Payment>> GetAllPaymentsAsync(int pageNumber, int pageSize);
        Task<PaymentModel?> GetPaymentByIdAsync(int paymentId);
        Task<List<PaymentModel?>> GetPaymentsByOrderIdAsync(int orderId);

        Task AddPaymentAsync(Payment payment);
        Task UpdatePaymentAsync(PaymentModel payment);
        Task DeletePaymentAsync(int paymentId);
    }
}