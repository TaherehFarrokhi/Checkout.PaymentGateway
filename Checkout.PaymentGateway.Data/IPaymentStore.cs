using System.Threading.Tasks;
using Checkout.PaymentGateway.Services.DomainObjects;

namespace Checkout.PaymentGateway.Services
{
    public interface IPaymentStore
    {
        Task SavePaymentAsync(Payment payment);
        Task<Payment> SavePaymentAsync(string paymentId);
    }
}