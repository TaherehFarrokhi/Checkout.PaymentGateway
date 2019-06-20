using System.Threading.Tasks;
using Checkout.PaymentGateway.Common;

namespace Checkout.PaymentGateway.Services.Stores
{
    public interface IPaymentStore
    {
        Task SavePaymentAsync(Payment payment);
        Task<Payment> GetPaymentAsync(string paymentId);
    }
}