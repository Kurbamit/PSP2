using ReactApp1.Server.Data.Repositories;
using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;

namespace ReactApp1.Server.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository giftCardRepository)
        {
            _paymentRepository = giftCardRepository;
        }

        public Task<PaginatedResult<Payment>> GetAllPayments(int pageSize, int pageNumber)
        {
            return _paymentRepository.GetAllPaymentsAsync(pageSize, pageNumber);
        }

        public Task<PaymentModel?> GetPaymentById(int paymentId)
        {
            return _paymentRepository.GetPaymentByIdAsync(paymentId);
        }
        public Task<List<PaymentModel?>> GetPaymentsByOrderId(int orderId)
        {
            return _paymentRepository.GetPaymentsByOrderIdAsync(orderId);
        }

        public Task CreateNewPayment(Payment payment)
        {
            return _paymentRepository.AddPaymentAsync(payment);
        }

        public Task UpdatePayment(PaymentModel payment)
        {
            return _paymentRepository.UpdatePaymentAsync(payment);
        }

        public Task DeletePayment(int paymentId)
        {
            return _paymentRepository.DeletePaymentAsync(paymentId);
        }
    }
}

