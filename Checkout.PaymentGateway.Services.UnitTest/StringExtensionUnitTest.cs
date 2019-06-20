using Xunit;

namespace Checkout.PaymentGateway.Services.UnitTest
{
    public class StringExtensionUnitTest
    {
        [Theory]
        [InlineData(null,10, '-',null)]
        [InlineData("", 10, '-',"")]
        [InlineData("123456", 10, '-',"123456")]
        [InlineData("1234567891234567", 4, '-', "------------4567")]
        public void MaskFromStart_ShouldMaskCorrectly(string value,  int displayCount, char mask, string expectedValue)
        {
            Assert.Equal(value.MaskFromStart(displayCount, mask), expectedValue);
        }
    }
}