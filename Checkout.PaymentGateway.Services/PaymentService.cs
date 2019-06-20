using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Common;
using Checkout.PaymentGateway.Services.BankClientApi;
using Checkout.PaymentGateway.Services.Responses;
using Checkout.PaymentGateway.Services.Stores;
using Checkout.PaymentGateway.Services.Validators;
using Microsoft.Extensions.Logging;

namespace Checkout.PaymentGateway.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IBankPaymentClient _bankPaymentClient;
        private readonly ILogger<PaymentService> _logger;
        private readonly IPaymentStore _paymentStore;
        private readonly IPaymentValidator _paymentValidator;

        public PaymentService(IBankPaymentClient bankPaymentClient, IPaymentStore paymentStore,
            IPaymentValidator paymentValidator,
            ILogger<PaymentService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _paymentValidator = paymentValidator ??
                                throw new ArgumentNullException(nameof(paymentValidator));
            _bankPaymentClient =
                bankPaymentClient ?? throw new ArgumentNullException(nameof(bankPaymentClient));
            _paymentStore = paymentStore ?? throw new ArgumentNullException(nameof(paymentStore));
        }

        public async Task<ClientResponse<PaymentResult>> ProcessPaymentAsync(PaymentRequest paymentRequest)
        {
            if (paymentRequest == null) throw new ArgumentNullException(nameof(paymentRequest));

            var validationResult = _paymentValidator.Validate(paymentRequest);
            if (!validationResult.IsValid)
                return ClientResponse<PaymentResult>.FromError(validationResult.ToString(), new PaymentResult(paymentRequest.PaymentRequestId));

            var payment = paymentRequest.ToPayment();

            try
            {
                await UpdatePaymentAsync(payment);

                var bankPaymentResult = await _bankPaymentClient.ProcessPaymentAsync(paymentRequest);

                payment.PaymentStatus = bankPaymentResult.Status ? PaymentStatus.Succeed : PaymentStatus.Declined;
                payment.PaymentReferenceId = bankPaymentResult.PaymentReferenceId;

                await UpdatePaymentAsync(payment);

                var paymentResult = new PaymentResult(payment.PaymentRequestId, payment.PaymentReferenceId);
                return bankPaymentResult.Status
                    ? ClientResponse<PaymentResult>.FromPayload(paymentResult)
                    : ClientResponse<PaymentResult>.FromError("Payment was unsuccessful.The bank returns unsuccessful response.", paymentResult);
            }
            catch (PaymentStoreException e)
            {
                _logger.LogError("Error in saving payment", e);

                // TODO: Can be handled in different strategies. E.g Revert the payment with bank or retry saving.

                return ClientResponse<PaymentResult>.FromError("Payment was unsuccessful.", new PaymentResult(payment.PaymentRequestId, payment.PaymentReferenceId));
            }
            catch (Exception e)
            {
                _logger.LogError("Error in process payment", e);

                return ClientResponse<PaymentResult>.FromError("Payment was unsuccessful.", new PaymentResult(payment.PaymentRequestId));
            }
        }

        public async Task<ClientResponse<Payment>> GetPaymentAsync(string paymentId)
        {
            var payment = await _paymentStore.GetPaymentAsync(paymentId);
            return ClientResponse<Payment>.FromPayload(payment);
        }

        private async Task UpdatePaymentAsync(Payment payment)
        {
            try
            {
                await _paymentStore.SavePaymentAsync(payment);
            }
            catch (Exception e)
            {
                throw new PaymentStoreException($"Error in updating payment in the store", e);
            }
        }
    }
}