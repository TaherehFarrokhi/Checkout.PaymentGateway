using System;
using System.Threading.Tasks;
using Checkout.PaymentGetway.Common;
using Microsoft.Extensions.Caching.Memory;

namespace Checkout.PaymentGateway.Services.Stores
{
    // TODO: Must be changed for more extensible and thread safe solution
    public class PaymentStore : IPaymentStore
    {
        private readonly IMemoryCache _memoryCache;

        public PaymentStore(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        public Task SavePaymentAsync(Payment payment)
        {
            payment.LastModifiedAt = DateTime.Now;
            _memoryCache.Set(payment.PaymentRequestId, payment);
            return Task.CompletedTask;
        }

        public Task<Payment> GetPaymentAsync(string paymentId)
        {
            return _memoryCache.TryGetValue<Payment>(paymentId, out var payment)
                ? Task.FromResult(payment)
                : Task.FromResult((Payment) null);
        }
    }
}