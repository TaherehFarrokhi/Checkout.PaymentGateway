using System;
using Microsoft.Extensions.Logging;
using Moq;

namespace Checkout.PaymentGateway.Services.UnitTest
{
    public static class LoggerHelper
    {
        public static Mock<ILogger<T>> LoggerMock<T>() where T : class
        {
            return new Mock<ILogger<T>>();
        }

        public static ILogger<T> Logger<T>() where T : class
        {
            return LoggerMock<T>().Object;
        }

        public static void VerifyLog<T>(this Mock<ILogger<T>> loggerMock, LogLevel level, string message, string failMessage = null)
        {
            loggerMock.VerifyLog(level, message, Times.Once(), failMessage);
        }
        public static void VerifyLog<T>(this Mock<ILogger<T>> loggerMock, LogLevel level, string message, Times times, string failMessage = null)
        {
            loggerMock.Verify(l => l.Log(level, It.IsAny<EventId>(), It.Is<Object>(o => o.ToString() == message), null, It.IsAny<Func<Object, Exception, String>>()), times, failMessage);
        }
    }
}