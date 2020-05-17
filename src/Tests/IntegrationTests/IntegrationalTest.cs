using ManageRates.Core.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading.Tasks;
using Xunit;

namespace Integration.Tests
{
    public class IntegrationalTest : IClassFixture<WebApplicationFactory<WebApi.Startup>>
    {
        private readonly WebApplicationFactory<WebApi.Startup> _factory;
        private readonly Mock<ITimeService> _timeServiceMock;
        private readonly DateTime _baseTime;

        public IntegrationalTest(WebApplicationFactory<WebApi.Startup> factory)
        {
            _factory = factory;
            _baseTime = new DateTime(2020, 01, 01);

            _timeServiceMock = new Mock<ITimeService>();

            _factory = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                   {
                       services.AddSingleton<ITimeService>(_timeServiceMock.Object);
                   });
                });
        }

        [Theory]
        [InlineData("/yesno/yes")]
        [InlineData("/endpoint")]
        public async Task CheckTwoTimesPerSecondEndpoints(string endpoint)
        {
            _timeServiceMock.Setup(t => t.GetUTC()).Returns(_baseTime);
            using var client = _factory.CreateClient();

            var answers = new[]
            {
                HttpStatusCode.OK,
                HttpStatusCode.OK,
                HttpStatusCode.TooManyRequests,
                HttpStatusCode.TooManyRequests
            };

            foreach (var answer in answers)
            {
                var profile = await client.GetAsync(endpoint);

                Assert.Equal(answer, profile.StatusCode);
            }

            _timeServiceMock.Reset();
        }

        [Theory]
        [InlineData("/user")]
        [InlineData("/yesno/no")]
        public async Task TestUserStrictionsEndpoints(string endpoint)
        {
            _timeServiceMock.Setup(t => t.GetUTC()).Returns(_baseTime);
            var server = _factory.Server;

            var answers = new[]
            {
                StatusCodes.Status200OK,
                StatusCodes.Status200OK,
                StatusCodes.Status429TooManyRequests,
                StatusCodes.Status429TooManyRequests
            };

            var principal1 = new GenericPrincipal(new GenericIdentity("User1"), null);
            var principal2 = new GenericPrincipal(new GenericIdentity("User2"), null);

            var principals = new[] { principal1, principal2 };


            foreach (var principal in principals)
                foreach (var answer in answers)
                {
                    var okResponse = await server.SendAsync(context =>
                    {
                        context.Request.Path = endpoint;
                        context.User = principal;
                    });

                    Assert.Equal(answer, okResponse.Response.StatusCode);
                }

            _timeServiceMock.Reset();
        }

        [Theory]
        [InlineData("/ip")]
        [InlineData("/yesno/fail")]
        public async Task TestIpRateForTwoDifferentIP(string endpoint)
        {
            _timeServiceMock.Setup(t => t.GetUTC()).Returns(_baseTime);
            var server = _factory.Server;

            var answers = new[]
            {
                StatusCodes.Status200OK,
                StatusCodes.Status200OK,
                StatusCodes.Status429TooManyRequests,
                StatusCodes.Status429TooManyRequests
            };

            var ipV4Array = new[] { IPAddress.Parse("127.168.1.31"), IPAddress.Parse("127.168.1.32") };

            foreach (var ipV4 in ipV4Array)
                foreach (var answer in answers)
                {
                    var okResponse = await server.SendAsync(context =>
                    {
                        context.Request.Path = endpoint;
                        context.Request.Method = HttpMethods.Get;
                        context.Connection.RemoteIpAddress = ipV4;
                    });

                    Assert.Equal(answer, okResponse.Response.StatusCode);
                }
         
            _timeServiceMock.Reset();
        }

        [Fact]
        public async Task TestDelegateRate()
        {
            using var client = _factory.CreateClient();

            for (int i = 0; i < 10; ++i)
            {
                var response = await client.GetAsync("/delegate");
                Assert.Equal(HttpStatusCode.TooManyRequests, response.StatusCode);
            }
        }
    }
}
