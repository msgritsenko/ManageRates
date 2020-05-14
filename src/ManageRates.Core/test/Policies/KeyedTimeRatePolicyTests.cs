using System;
using ManageRates.Core.Abstractions;
using ManageRates.Core.Policies;
using Moq;
using Xunit;

namespace ManageRates.Core.Tests.Policies
{
    public class KeyedTimeRatePolicyTests
    {
        public void IsPermitted_ConstantTime_ReturnsTrueFirstTimesForeachKey()
        {
            DateTime testTime = new DateTime(2020, 1, 1);

            var timeServiceMock = new Mock<ITimeService>();
            timeServiceMock.Setup(t => t.GetUTC()).Returns(testTime);

            var policy = new KeyedTimeRatePolicy(TimeSpan.FromSeconds(1), 2, timeServiceMock.Object);

            Assert.True(policy.IsPermitted("1"));
            Assert.True(policy.IsPermitted("1"));
            Assert.False(policy.IsPermitted("1"));

            Assert.True(policy.IsPermitted("2"));
            Assert.True(policy.IsPermitted("2"));
            Assert.False(policy.IsPermitted("2"));
        }

        public void IsPermitted_IncreasingTime_ReturnsTrueAlways()
        {
            DateTime testTime = new DateTime(2020, 1, 1);

            var timeServiceMock = new Mock<ITimeService>();
            timeServiceMock.Setup(t => t.GetUTC()).Returns(() => (testTime = testTime.AddSeconds(2)));

            var policy = new KeyedTimeRatePolicy(TimeSpan.FromSeconds(1), 2, timeServiceMock.Object);

            Assert.True(policy.IsPermitted("1"));
            Assert.True(policy.IsPermitted("1"));
            Assert.True(policy.IsPermitted("1"));
        }
    }
}
