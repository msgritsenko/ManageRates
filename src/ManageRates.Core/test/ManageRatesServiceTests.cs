using ManageRates.Core.Abstractions;
using ManageRates.Core.Model;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ManageRates.Core.Tests
{
    public class ManageRatesServiceTests
    {
        private const string key = "123";

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Process_PolicyAlwaysValue_ReturnValue(bool value)
        {
            var policyMock = new Mock<IManageRatePolicy>();
            policyMock.Setup(p => p.IsPermitted(It.IsAny<string>())).ReturnsAsync(value);

            var service = new ManageRatesService();

            var request = new ManageRatesRequest(key, policyMock.Object);
            var result = await service.Process(request);

            Assert.Equal(value, result.Permitted);
        }

        [Fact]
        public async Task Process_PolicyReturnIfRequestIsOdd_ReturnFalseThenTrue()
        {
            var policyMock = new Mock<IManageRatePolicy>();
            int requestCounter = 0;
            policyMock.Setup(p => p.IsPermitted(It.IsAny<string>())).ReturnsAsync(requestCounter++ % 2 == 1);

            var service = new ManageRatesService();
            var request = new ManageRatesRequest(key, policyMock.Object);

            var result1 = await service.Process(request);
            Assert.False(result1.Permitted);

            var result2 = await service.Process(request);
            Assert.False(result2.Permitted);
        }
    }
}
