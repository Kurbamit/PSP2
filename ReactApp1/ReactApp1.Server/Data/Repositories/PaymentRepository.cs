using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Extensions;
using ReactApp1.Server.Models;
using ReactApp1.Server.Models.Enums;
using ReactApp1.Server.Models.Models.Base;
using ReactApp1.Server.Models.Models.Domain;
using Stripe;

namespace ReactApp1.Server.Data.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDbContext _context;

        public PaymentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResult<Payment>> GetAllPaymentsAsync(int pageNumber, int pageSize)
        {
            var totalPayments = await _context.Set<Payment>().CountAsync();
            var totalPages = (int)Math.Ceiling(totalPayments / (double)pageSize);

            var payments = await _context.Set<Payment>()
                .OrderBy(payment => payment.PaymentId)
                .Paginate(pageNumber, pageSize)
                .ToListAsync();

            return new PaginatedResult<Payment>
            {
                Items = payments,
                TotalPages = totalPages,
                TotalItems = totalPayments,
                CurrentPage = pageNumber
            };
        }

        public async Task<PaymentModel?> GetPaymentByIdAsync(int paymentId)
        {
            var payment = await _context.Payments
                .Where(p => p.PaymentId == paymentId)
                .Select(p => new PaymentModel
                {
                    PaymentId = p.PaymentId,
                    OrderId = p.OrderId,
                    Type = p.Type,
                    Value = p.Value,
                    ReceiveTime = p.ReceiveTime,
                    GiftCardId = p.GiftCardId
                }).FirstOrDefaultAsync();

            return payment;
        }
        public async Task<List<PaymentModel?>> GetPaymentsByOrderIdAsync(int orderId)
        {
            var payments = await _context.Payments
                .Where(p => p.OrderId == orderId)
                .Select(p => new PaymentModel
                {
                    PaymentId = p.PaymentId,
                    OrderId = p.OrderId,
                    Type = p.Type,
                    Value = p.Value,
                    ReceiveTime = p.ReceiveTime,
                    GiftCardId = p.GiftCardId
                }).ToListAsync();

            return payments;
        }
        public async Task AddPaymentAsync(PaymentModel payment)
        {
            var newPayment = new Payment
            {
                OrderId = payment.OrderId,
                Type = payment.Type,
                Value = payment.Value,
                ReceiveTime = payment.ReceiveTime,
                GiftCardId = payment.GiftCardId,
            };

            try
            {
                await _context.Set<Payment>().AddAsync(newPayment);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw new Exception("An error occurred while adding a new payment to the database.", e);
            }
        }

        public async Task UpdatePaymentAsync(PaymentModel paymentModel)
        {
            try
            {
                var existingPayment = await _context.Set<Payment>()
                    .FirstOrDefaultAsync(p => p.PaymentId == paymentModel.PaymentId);

                if (existingPayment == null)
                {
                    throw new KeyNotFoundException($"Payment with ID {paymentModel.PaymentId} not found.");
                }

                paymentModel.MapUpdate(existingPayment);

                _context.Set<Payment>().Update(existingPayment);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw new Exception($"An error occurred while updating the payment: {paymentModel.PaymentId}.", e);
            }
        }

        public async Task DeletePaymentAsync(int paymentId)
        {
            try
            {
                _context.Set<Payment>().Remove(new Payment
                {
                    PaymentId = paymentId
                });

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw new Exception($"An error occurred while deleting the payment {paymentId} from the database.", e);
            }
        }
        public async Task<PaymentIntent> CreatePaymentIntentAsync(decimal amount, string currency)
        {
            StripeConfiguration.ApiKey = "sk_test_51QUk2yJ37W5f2NTsZ7NhBFoswfTBo6Bw39BjvxByuY4VopIvVoDenzgFjYptV96a4OiSETcbGJX0g6NkJilU8n4c003P1SMJyP";
            
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long) (amount * 100), // convert to cents
                Currency = currency,
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true,
                },
            };

            var service = new PaymentIntentService();
            var paymentIntent = await service.CreateAsync(options);

            return paymentIntent;
        }

    }
}
