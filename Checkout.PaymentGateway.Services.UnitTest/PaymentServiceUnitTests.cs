using System;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Services.BankClientApi;
using Checkout.PaymentGateway.Services.Stores;
using Checkout.PaymentGateway.Services.Validators;
using Checkout.PaymentGetway.Common;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Checkout.PaymentGateway.Services.UnitTest
{
    public class PaymentServiceUnitTests
    {
        public PaymentServiceUnitTests()
        {
            _bankPaymentClientMock = new Mock<IBankPaymentClient>();
            _loggerMock = LoggerHelper.LoggerMock<PaymentService>();
            _paymentStoreMock = new Mock<IPaymentStore>();
            _paymentValidatorMock = new Mock<IPaymentValidator>();

            _subject = new PaymentService(_bankPaymentClientMock.Object, _paymentStoreMock.Object,
                _paymentValidatorMock.Object, _loggerMock.Object);
        }

        private readonly Mock<IBankPaymentClient> _bankPaymentClientMock;
        private readonly Mock<ILogger<PaymentService>> _loggerMock;
        private readonly Mock<IPaymentStore> _paymentStoreMock;
        private readonly Mock<IPaymentValidator> _paymentValidatorMock;
        private readonly PaymentService _subject;

        [Fact]
        public async Task ProcessPaymentAsync_ShouldReturnsFailedResponse_WhenPaymentRequestIsNotValid()
        {
            // Arrange
            _paymentValidatorMock.Setup(x => x.Validate(It.IsAny<PaymentRequest>())).Returns(new ValidationResult
                {Errors = {new ValidationFailure(nameof(PaymentRequest.CardNumber), "")}});
            // Act
            var clientResponse = await _subject.ProcessPaymentAsync(new PaymentRequest());

            //Assert
            Assert.False(clientResponse.Success);
            Assert.Null(clientResponse.Payload.PaymentReferenceId);
        }

        [Fact]
        public async Task ProcessPaymentAsync_ShouldReturnsFailedResponse_WhenBankPaymentClientReturnsException()
        {
            // Arrange
            _paymentValidatorMock.Setup(x => x.Validate(It.IsAny<PaymentRequest>())).Returns(new ValidationResult());
            _bankPaymentClientMock.Setup(x => x.ProcessPaymentAsync(It.IsAny<PaymentRequest>()))
                .ThrowsAsync(new Exception());
            // Act
            var clientResponse = await _subject.ProcessPaymentAsync(new PaymentRequest());

            //Assert
            _loggerMock.VerifyLog(LogLevel.Error, "Error in process payment", Times.Once());
            Assert.False(clientResponse.Success);
            Assert.Equal("Payment was unsuccessful.", clientResponse.ErrorMessage);
            Assert.Null(clientResponse.Payload.PaymentReferenceId);
        }

        [Fact]
        public async Task ProcessPaymentAsync_ShouldReturnsFailedResponse_WhenPaymentStoreReturnsException()
        {
            // Arrange
            _paymentValidatorMock.Setup(x => x.Validate(It.IsAny<PaymentRequest>())).Returns(new ValidationResult());
            var bankPaymentResult = new BankPaymentResult{Status = true, PaymentReferenceId = "123456789012345"};
            _bankPaymentClientMock.Setup(x => x.ProcessPaymentAsync(It.IsAny<PaymentRequest>())).ReturnsAsync(bankPaymentResult);
            _paymentStoreMock.Setup(x => x.SavePaymentAsync(It.IsAny<Payment>())).ThrowsAsync(new Exception());

            // Act
            var clientResponse = await _subject.ProcessPaymentAsync(new PaymentRequest());

            //Assert
            _loggerMock.VerifyLog(LogLevel.Error, "Error in saving payment", Times.Once());
            Assert.False(clientResponse.Success);
            Assert.Equal("Payment was unsuccessful.", clientResponse.ErrorMessage);
            Assert.Null(clientResponse.Payload.PaymentReferenceId);
        }

        [Fact]
        public async Task ProcessPaymentAsync_ShouldReturnsFailedResponse_WhenBankPaymentApiReturnSuccessResponseAndErrorHappendInSavingPayment()
        {
            // Arrange
            _paymentValidatorMock.Setup(x => x.Validate(It.IsAny<PaymentRequest>())).Returns(new ValidationResult());
            var bankPaymentResult = new BankPaymentResult{Status = true, PaymentReferenceId = "123456789012345"};
            _bankPaymentClientMock.Setup(x => x.ProcessPaymentAsync(It.IsAny<PaymentRequest>())).ReturnsAsync(bankPaymentResult);
            _paymentStoreMock.SetupSequence(x => x.SavePaymentAsync(It.IsAny<Payment>())).Returns(Task.CompletedTask).Throws(new Exception());

            // Act
            var clientResponse = await _subject.ProcessPaymentAsync(new PaymentRequest());

            //Assert
            _loggerMock.VerifyLog(LogLevel.Error, "Error in saving payment", Times.Once());
            Assert.False(clientResponse.Success);
            Assert.Equal("Payment was unsuccessful.", clientResponse.ErrorMessage);
            Assert.NotEmpty(clientResponse.Payload.PaymentReferenceId);
        }

        [Fact]
        public async Task ProcessPaymentAsync_ShouldReturnsFailedResponse_WhenBankPaymentApiReturnFailedResponse()
        {
            // Arrange
            _paymentValidatorMock.Setup(x => x.Validate(It.IsAny<PaymentRequest>())).Returns(new ValidationResult());
            var bankPaymentResult = new BankPaymentResult { Status = false};
            _bankPaymentClientMock.Setup(x => x.ProcessPaymentAsync(It.IsAny<PaymentRequest>())).ReturnsAsync(bankPaymentResult);

            // Act
            var clientResponse = await _subject.ProcessPaymentAsync(new PaymentRequest());

            //Assert
            Assert.False(clientResponse.Success);
            Assert.Equal("Payment was unsuccessful.The bank returns unsuccessful response.",clientResponse.ErrorMessage);
        }

        [Fact]
        public async Task ProcessPaymentAsync_ShouldReturnsFailedResponse_WhenBankPaymentApiReturnSuccessResponse()
        {
            // Arrange
            _paymentValidatorMock.Setup(x => x.Validate(It.IsAny<PaymentRequest>())).Returns(new ValidationResult());
            var bankPaymentResult = new BankPaymentResult { Status = true, PaymentReferenceId = "1234567890"};
            _bankPaymentClientMock.Setup(x => x.ProcessPaymentAsync(It.IsAny<PaymentRequest>())).ReturnsAsync(bankPaymentResult);

            // Act
            var clientResponse = await _subject.ProcessPaymentAsync(new PaymentRequest());

            //Assert
            Assert.True(clientResponse.Success);
            Assert.Null(clientResponse.ErrorMessage);
        }
    }
}