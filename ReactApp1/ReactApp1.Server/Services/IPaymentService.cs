using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;
using Stripe;

namespace ReactApp1.Server.Services;

public interface IPaymentService
{
    Task<PaginatedResult<Payment>> GetAllPayments(int pageSize, int pageNumber);
    Task<PaymentModel?> GetPaymentById(int paymentId);
    Task<List<PaymentModel?>> GetPaymentsByOrderId(int orderId);
    Task CreateNewPayment(PaymentModel payment);
    Task UpdatePayment(PaymentModel payment);
    Task DeletePayment(int paymentId);
    Task<PaymentIntent> CreatePaymentIntent(decimal amount, string currency);
    Task RefundPaymentIntent(string stripePaymentId);
}