using ManageRates.Core.Abstractions;
using ManageRates.Core.Model;
using Moq;
using Xunit;

namespace ManageRates.Core.Tests
{
    public class ManageRatesServiceTests
    {
        private const string key = "123";

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Process_PolicyAlwaysValue_ReturnValue(bool value)
        {
            var policyMock = new Mock<IKeyedManageRatePolicy>();
            policyMock.Setup(p => p.IsPermitted(It.IsAny<string>())).Returns(value);

            var service = new ManageRatesService();

            var request = new KeyedManageRatesRequest(key, policyMock.Object);
            var result = service.Process(request);

            Assert.Equal(value, result.Permitted);
        }

        [Fact]
        public void Process_PolicyReturnIfRequestIsOdd_ReturnFalseThenTrue()
        {
            var policyMock = new Mock<IKeyedManageRatePolicy>();
            int requestCounter = 0;
            policyMock.Setup(p => p.IsPermitted(It.IsAny<string>())).Returns(requestCounter++ % 2 == 1);

            var service = new ManageRatesService();
            var request = new KeyedManageRatesRequest(key, policyMock.Object);

            var result1 = service.Process(request);
            Assert.False(result1.Permitted);

            var result2 = service.Process(request);
            Assert.False(result2.Permitted);
        }
    }
}
