using System.Threading.Tasks;
using Checkout.PaymentGetway.Common;

namespace Checkout.PaymentGateway.Services.Stores
{
    public interface IPaymentStore
    {
        Task SavePaymentAsync(Payment payment);
        Task<Payment> GetPaymentAsync(string paymentId);
    }
}