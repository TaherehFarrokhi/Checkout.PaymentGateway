using System;
using System.Collections.Generic;
using System.Linq;
using Checkout.PaymentGateway.Common;
using Checkout.PaymentGateway.Services.Validators;
using Xunit;

namespace Checkout.PaymentGateway.Services.UnitTest
{
    public class PaymentValidatorUnitTests
    {
        public PaymentValidatorUnitTests()
        {
            _subject = new PaymentValidator();
        }

        private readonly PaymentValidator _subject;

        public static IEnumerable<object[]> Data =>
            new List<object[]>
            {
                new object[]
                {
                    new PaymentRequest(),
                    new List<string>
                    {
                        "'Card Number' must not be empty.", "'Expiry Date' must not be empty.",
                        "ExpiryDate should be in future.", "Amount should have value greater than zero.",
                        "Currency can not be empty.", "'Cvv' must not be empty."
                    }
                },
                new object[]
                {
                    new PaymentRequest {CardNumber = "123456"},
                    new List<string>
                    {
                        "CardNumber can not be empty and must have 16 digits.", "'Expiry Date' must not be empty.",
                        "ExpiryDate should be in future.", "Amount should have value greater than zero.",
                        "Currency can not be empty.", "'Cvv' must not be empty."
                    }
                },
                new object[]
                {
                    new PaymentRequest {CardNumber = "1234567891234567", ExpiryDate = DateTime.Now.AddHours(-1)},
                    new List<string>
                    {
                        "ExpiryDate should be in future.", "Amount should have value greater than zero.",
                        "Currency can not be empty.", "'Cvv' must not be empty."
                    }
                },
                new object[]
                {
                    new PaymentRequest {CardNumber = "1234567891234567", ExpiryDate = DateTime.Now.AddYears(1)},
                    new List<string>
                    {
                        "Amount should have value greater than zero.",
                        "Currency can not be empty.", "'Cvv' must not be empty."
                    }
                },
                new object[]
                {
                    new PaymentRequest
                        {CardNumber = "1234567891234567", ExpiryDate = DateTime.Now.AddYears(1), Amount = 20},
                    new List<string>
                    {
                        "Currency can not be empty.",
                        "'Cvv' must not be empty."
                    }
                },
                new object[]
                {
                    new PaymentRequest
                    {
                        CardNumber = "1234567891234567", ExpiryDate = DateTime.Now.AddYears(1), Amount = 20,
                        Currency = "USD"
                    },
                    new List<string>
                    {
                        "'Cvv' must not be empty."
                    }
                },
                new object[]
                {
                    new PaymentRequest
                    {
                        CardNumber = "1234567891234567", ExpiryDate = DateTime.Now.AddYears(1), Amount = 20,
                        Currency = "USD", Cvv = "12"
                    },
                    new List<string>
                    {
                        "Cvv can not be empty and must have 3 digits."
                    }
                }
            };

        [Theory]
        [MemberData(nameof(Data))]
        public void Validate_ShouldReturnsExpectedErrorMessage_WhenPaymentRequestIsNotValid(
            PaymentRequest paymentRequest, List<string> expectedErrorMessages)
        {
            // Arrange
            // Act
            var validationResult = _subject.Validate(paymentRequest);
            // Assert
            Assert.False(validationResult.IsValid);
            Assert.Equal(expectedErrorMessages, validationResult.Errors.Select(x => x.ErrorMessage).ToList());
        }

        [Fact]
        public void Validate_ShouldReturnsNoErrors_WhenPaymentRequestIsValid()
        {
            // Arrange
            var paymentRequest = new PaymentRequest
            {
                CardNumber = "1234567891234567", ExpiryDate = DateTime.Now.AddYears(1), Amount = 20,
                Currency = "USD", Cvv = "123"
            };

            // Act
            var validationResult = _subject.Validate(paymentRequest);

            // Assert
            Assert.True(validationResult.IsValid);
            Assert.Equal(0, validationResult.Errors.Count);
        }

        [Fact]
        public void Validate_ShouldReturnsException_WhenPaymentRequestIsNull()
        {
            // Arrange
            
            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(() => _subject.Validate(null));
        }
    }
}