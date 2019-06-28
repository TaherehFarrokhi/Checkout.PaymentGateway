using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Common;
using Newtonsoft.Json;
using Xunit;

namespace Checkout.PaymentGateway.Api.IntegrationTest
{
    public class PaymentControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Startup>>, IDisposable
    {
        public PaymentControllerIntegrationTests(CustomWebApplicationFactory<Startup> factory)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            _client = factory.CreateClient();
        }

        public void Dispose()
        {
            _client?.Dispose();
        }

        private readonly HttpClient _client;

        [Fact]
        public async Task ShouldCreatePayment_WhenPaymentRequestIsValid()
        {
            using (var stringContent = new StringContent(
                JsonConvert.SerializeObject(new PaymentRequest
                {
                    CardNumber = "1234567891234567",
                    Amount = 10,
                    Currency = "EUR",
                    ExpiryDate = DateTime.Now.AddYears(1),
                    Cvv = "123"
                }),
                Encoding.UTF8,
                "application/json"))
            {
                var httpResponse = await _client.PostAsync("/api/v1/payment", stringContent);
                var stringResponse = await httpResponse.Content.ReadAsStringAsync();
                var paymentResult = JsonConvert.DeserializeObject<PaymentResult>(stringResponse);
                Assert.NotEmpty(paymentResult.PaymentReferenceId);
                Assert.NotEmpty(paymentResult.PaymentRequestId);
            }
        }

        [Fact]
        public async Task ShouldGetPayment_WhenPaymentRequestIsSavedCorrectly()
        {
            var paymentRequest = new PaymentRequest
            {
                CardNumber = "1234567891234567",
                Amount = 10,
                Currency = "EUR",
                ExpiryDate = DateTime.Now.AddYears(1),
                Cvv = "123"
            };

            using (var stringContent = new StringContent(
                JsonConvert.SerializeObject(paymentRequest),
                Encoding.UTF8,
                "application/json"))
            {
                var paymentCreationResponse = await _client.PostAsync("/api/v1/payment", stringContent);
                var stringResponse = await paymentCreationResponse.Content.ReadAsStringAsync();
                var paymentResult = JsonConvert.DeserializeObject<PaymentResult>(stringResponse);

                var paymentResponse = await _client.GetAsync($"/api/v1/payment/{paymentResult.PaymentRequestId}");

                stringResponse = await paymentResponse.Content.ReadAsStringAsync();
                var payment = JsonConvert.DeserializeObject<Payment>(stringResponse);

                Assert.Equal(paymentRequest.Amount,payment.Amount);
                Assert.Equal(payment.Currency, paymentRequest.Currency);
                Assert.Equal(PaymentStatus.Succeed, payment.PaymentStatus);
            }
        }
    }
}