using System.Threading.Tasks;
using Checkout.PaymentGateway.Services.DomainObjects;

namespace Checkout.PaymentGateway.Services
{
    public class PaymentStore : IPaymentStore
    {
        public Task SavePaymentAsync(Payment payment)
        {
            throw new System.NotImplementedException();
        }

        public Task<Payment> SavePaymentAsync(string paymentId)
        {
            throw new System.NotImplementedException();
        }
    }
}