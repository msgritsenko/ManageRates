using ManageRates.AspnetCore.Abstractions;
using ManageRates.AspnetCore.Configuration;
using ManageRates.Core.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ManageRates.AspnetCore.Tests
{
    public class ManageRatesMiddlewareTests
    {
        [Fact]
        public void Ctor_ManageRatesServiceIsNull_ThrowsException()
        {
            RequestDelegate requestDelegate = c => Task.CompletedTask;
            var options = Options.Create(new ManageRatesConfiguration());
            Assert.Throws<ArgumentNullException>("manageRatesService", () => new ManageRatesMiddleware(requestDelegate, options, null));
        }

        [Fact]
        public async Task InvokeAsync_HttpContextIsNull_ThrowsException()
        {
            var manageRatesServiceMock = new Mock<IManageRatesService>();
            RequestDelegate requestDelegate = c => Task.CompletedTask;
            var options = Options.Create(new ManageRatesConfiguration());

            var middleware = new ManageRatesMiddleware(requestDelegate, options, manageRatesServiceMock.Object);

            await Assert.ThrowsAsync<ArgumentNullException>("httpContext", () => middleware.InvokeAsync(null));
            manageRatesServiceMock.VerifyNoOtherCalls();
        }

        [Fact] 
        public async Task InvokeAsync_ManageRatesServiceReturnsFalse_ReturnsTooManyRequests()
        {
            var manageRatesServiceMock = new Mock<IManageRatesService>();
            manageRatesServiceMock
                .Setup(s => s.Process(It.IsAny<HttpContext>(), It.IsAny<ManageRatesConfiguration>()))
                    .Returns(new ManageRatesResult(false));
            RequestDelegate requestDelegate = c => Task.CompletedTask;

            var options = Options.Create(new ManageRatesConfiguration());

            var middleware = new ManageRatesMiddleware(requestDelegate, options, manageRatesServiceMock.Object);
            var http = new DefaultHttpContext();

            await middleware.InvokeAsync(http);

            Assert.Equal(StatusCodes.Status429TooManyRequests, http.Response.StatusCode);
        }

        [Fact]
        public void InvokeAsync_ManageRatesServiceReturnsTrue_ReturnsTaskFromNextRequest()
        {
            var manageRatesServiceMock = new Mock<IManageRatesService>();
            manageRatesServiceMock
                .Setup(s => s.Process(It.IsAny<HttpContext>(), It.IsAny<ManageRatesConfiguration>()))
                    .Returns(new ManageRatesResult(true));
            
            Task expectedTask = Task.FromResult(7);
            RequestDelegate requestDelegate = c => expectedTask;
            var options = Options.Create(new ManageRatesConfiguration());
            var middleware = new ManageRatesMiddleware(requestDelegate, options, manageRatesServiceMock.Object);
            var http = new DefaultHttpContext();

            var actualTask = middleware.InvokeAsync(http);

            Assert.Same(expectedTask, actualTask);
        }
    }
}