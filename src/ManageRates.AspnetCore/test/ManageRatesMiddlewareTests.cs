using ManageRates.AspnetCore.Abstractions;
using ManageRates.Core.Model;
using Microsoft.AspNetCore.Http;
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

            Assert.Throws<ArgumentNullException>("manageRatesService", () => new ManageRatesMiddleware(requestDelegate, null));
        }

        [Fact]
        public async Task InvokeAsync_HttpContextIsNull_ThrowsException()
        {
            var manageRatesServiceMock = new Mock<IManageRatesService>();
            RequestDelegate requestDelegate = c => Task.CompletedTask;

            var middleware = new ManageRatesMiddleware(requestDelegate, manageRatesServiceMock.Object);

            await Assert.ThrowsAsync<ArgumentNullException>("httpContext", () => middleware.InvokeAsync(null));
            manageRatesServiceMock.VerifyNoOtherCalls();
        }

        [Fact] 
        public async Task InvokeAsync_ManageRatesServiceReturnsFalse_ReturnsTooManyRequests()
        {
            var manageRatesServiceMock = new Mock<IManageRatesService>();
            manageRatesServiceMock.Setup(s => s.Process(It.IsAny<HttpContext>())).Returns(new ManageRatesResult(false));
            RequestDelegate requestDelegate = c => Task.CompletedTask;

            var middleware = new ManageRatesMiddleware(requestDelegate, manageRatesServiceMock.Object);
            var http = new DefaultHttpContext();

            await middleware.InvokeAsync(http);

            Assert.Equal(StatusCodes.Status429TooManyRequests, http.Response.StatusCode);
        }

        [Fact]
        public async Task InvokeAsync_ManageRatesServiceReturnsTrue_ReturnsTaskFromNextRequest()
        {
            var manageRatesServiceMock = new Mock<IManageRatesService>();
            manageRatesServiceMock.Setup(s => s.Process(It.IsAny<HttpContext>())).Returns(new ManageRatesResult(true));
            
            Task expectedTask = Task.FromResult(7);
            RequestDelegate requestDelegate = c => expectedTask;

            var middleware = new ManageRatesMiddleware(requestDelegate, manageRatesServiceMock.Object);
            var http = new DefaultHttpContext();

            var actualTask = middleware.InvokeAsync(http);

            Assert.Same(expectedTask, actualTask);
        }
    }
}