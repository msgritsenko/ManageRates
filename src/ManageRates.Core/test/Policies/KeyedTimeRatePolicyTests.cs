using System;
using System.Collections.Generic;
using ManageRates.Core.Abstractions;
using ManageRates.Core.Policies;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Xunit;

namespace ManageRates.Core.Tests.Policies
{
    public class KeyedTimeRatePolicyTests
    {
        [Fact]
        public void IsPermitted_ConstantTime_ReturnsTrueFirstTimesForeachKey()
        {
            // Arrange
            DateTime testTime = new DateTime(2020, 1, 1);

            var timeServiceMock = new Mock<ITimeService>();
            timeServiceMock.Setup(t => t.GetUTC()).Returns(testTime);
            var memoryCacheMock = new Mock<IMemoryCache>();
            object queue1 = new Queue<DateTime>();
            object queue2 = new Queue<DateTime>();
            memoryCacheMock.Setup(m => m.TryGetValue(It.IsIn("1"), out queue1)).Returns(true);
            memoryCacheMock.Setup(m => m.TryGetValue(It.IsIn("2"), out queue2)).Returns(true);

            // Act
            var policy = new KeyedTimeRatePolicy(TimeSpan.FromSeconds(1), 2);

            // Assert
            Assert.True(policy.IsPermitted("1", timeServiceMock.Object, memoryCacheMock.Object));
            Assert.True(policy.IsPermitted("1", timeServiceMock.Object, memoryCacheMock.Object));
            Assert.False(policy.IsPermitted("1", timeServiceMock.Object, memoryCacheMock.Object));

            Assert.True(policy.IsPermitted("2", timeServiceMock.Object, memoryCacheMock.Object));
            Assert.True(policy.IsPermitted("2", timeServiceMock.Object, memoryCacheMock.Object));
            Assert.False(policy.IsPermitted("2", timeServiceMock.Object, memoryCacheMock.Object));
        }

        [Fact]
        public void IsPermitted_IncreasingTime_ReturnsTrueAlways()
        {
            // Arrange
            DateTime testTime = new DateTime(2020, 1, 1);

            var timeServiceMock = new Mock<ITimeService>();
            timeServiceMock.Setup(t => t.GetUTC()).Returns(() => (testTime = testTime.AddSeconds(2)));

            var memoryCacheMock = new Mock<IMemoryCache>();
            object queue1 = new Queue<DateTime>();
            memoryCacheMock.Setup(m => m.TryGetValue(It.IsIn("1"), out queue1)).Returns(true);

            // Act
            var policy = new KeyedTimeRatePolicy(TimeSpan.FromSeconds(1), 2);

            // Assert
            Assert.True(policy.IsPermitted("1", timeServiceMock.Object, memoryCacheMock.Object));
            Assert.True(policy.IsPermitted("1", timeServiceMock.Object, memoryCacheMock.Object));
            Assert.True(policy.IsPermitted("1", timeServiceMock.Object, memoryCacheMock.Object));
        }
    }
}
