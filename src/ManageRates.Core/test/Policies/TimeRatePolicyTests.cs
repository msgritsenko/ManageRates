using System;
using ManageRates.Core.Abstractions;
using ManageRates.Core.Policies;
using Moq;
using Xunit;

namespace ManageRates.Core.Tests.Policies
{
    public class TimeRatePolicyTests
    {
        [Fact]
        public void IsPermitted_ConstantTime_ReturnsTrueFirstTimesForeachKey()
        {
            DateTime testTime = new DateTime(2020, 1, 1);

            var timeServiceMock = new Mock<ITimeService>();
            timeServiceMock.Setup(t => t.GetUTC()).Returns(testTime);

            var policy = new TimeRatePolicy(TimeSpan.FromSeconds(1), 2);

            Assert.True(policy.IsPermitted(timeServiceMock.Object));
            Assert.True(policy.IsPermitted(timeServiceMock.Object));
            Assert.False(policy.IsPermitted(timeServiceMock.Object));
        }

        [Fact]
        public void IsPermitted_IncreasingTime_ReturnsTrueAlways()
        {
            DateTime testTime = new DateTime(2020, 1, 1);

            var timeServiceMock = new Mock<ITimeService>();
            timeServiceMock.Setup(t => t.GetUTC()).Returns(() => (testTime = testTime.AddSeconds(2)));

            var policy = new TimeRatePolicy(TimeSpan.FromSeconds(1), 2);

            Assert.True(policy.IsPermitted(timeServiceMock.Object));
            Assert.True(policy.IsPermitted(timeServiceMock.Object));
            Assert.True(policy.IsPermitted(timeServiceMock.Object));
        }
    }
}
