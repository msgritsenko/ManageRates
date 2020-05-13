using ManageRates.Core.Model;
using Xunit;

namespace ManageRates.Core.Tests.Models
{
    public class ManageRatesRequestTests
    {
        [Fact]
        public void Ctor_SetValues_ValuesCorrect()
        {
            var request = new ManageRatesRequest("0123456789", null);

            Assert.Equal("0123456789", request.Key);
            Assert.Null(request.Policy);
        }
    }
}
