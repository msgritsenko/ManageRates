using ManageRates.Core.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Net;
using System.Security.Principal;
using System.Threading.Tasks;
using Xunit;

namespace Integration.Tests
{
    public class IntegrationTest : IClassFixture<WebApplicationFactory<WebApi.Startup>>
    {
        private readonly DateTime _baseTime;
        private readonly Mock<ITimeService> _timeServiceMock;
        private readonly WebApplicationFactory<WebApi.Startup> _factory;

        public IntegrationTest(WebApplicationFactory<WebApi.Startup> factory)
        {
            _baseTime = new DateTime(2020, 01, 01);
            _timeServiceMock = new Mock<ITimeService>();

            _factory = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.AddSingleton(_timeServiceMock.Object);
                    });
                });
        }

        [Theory]
        [InlineData("/ControllerAttribute")]
        [InlineData("/MethodAttributes/endpoint")]
        [InlineData("/endpoint")]
        public async Task TestTwoTimesPerSecondEndpoints(string endpoint)
        {
            _timeServiceMock.Setup(t => t.GetUTC()).Returns(_baseTime);
            using var client = _factory.CreateClient();

            var tests = new[]
            {
                HttpStatusCode.OK,
                HttpStatusCode.OK,
                HttpStatusCode.TooManyRequests,
                HttpStatusCode.TooManyRequests
            };

            foreach (var test in tests)
            {
                var response = await client.GetAsync(endpoint);

                Assert.Equal(test, response.StatusCode);
            }

            _timeServiceMock.Reset();
        }

        [Theory]
        [InlineData("/user")]
        [InlineData("/MethodAttributes/user")]
        public async Task TestkTwoTimesPerSecondUserStrictionsEndpoints(string endpoint)
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
                    var response = await server.SendAsync(context =>
                    {
                        context.Request.Path = endpoint;
                        context.User = principal;
                    });

                    Assert.Equal(answer, response.Response.StatusCode);
                }

            _timeServiceMock.Reset();
        }

        [Theory]
        [InlineData("/ip")]
        [InlineData("/MethodAttributes/ip")]
        public async Task TestkTwoTimesPerSecondIpStrictionsEndpoints(string endpoint)
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
                    var response = await server.SendAsync(context =>
                    {
                        context.Request.Path = endpoint;
                        context.Request.Method = HttpMethods.Get;
                        context.Connection.RemoteIpAddress = ipV4;
                    });

                    Assert.Equal(answer, response.Response.StatusCode);
                }

            _timeServiceMock.Reset();
        }

        [Fact]
        public async Task TestBlockedByDelegate()
        {
            _timeServiceMock.Setup(t => t.GetUTC()).Returns(_baseTime);
            using var client = _factory.CreateClient();

            for (int i = 0; i < 10; ++i)
            {
                var response = await client.GetAsync("/delegate");
                Assert.Equal(HttpStatusCode.TooManyRequests, response.StatusCode);
            }

            _timeServiceMock.Reset();
        }

        [Fact]
        public async Task TestTwoTimesPerSecondForUserStrictionRegexPolicy()
        {
            _timeServiceMock.Setup(t => t.GetUTC()).Returns(_baseTime);
            var server = _factory.Server;

            var tests = new[]
            {
                (code :StatusCodes.Status200OK, endpoint: "/api/raw/userfunction"),
                (code :StatusCodes.Status200OK, endpoint: "/api/raw/functionUser"),
                (code :StatusCodes.Status429TooManyRequests, endpoint: "/api/raw/userfunction"),
                (code :StatusCodes.Status429TooManyRequests, endpoint: "/api/raw/functionUser")
            };

            var principal1 = new GenericPrincipal(new GenericIdentity("User1"), null);
            var principal2 = new GenericPrincipal(new GenericIdentity("User2"), null);

            var principals = new[] { principal1, principal2 };


            foreach (var principal in principals)
                foreach (var test in tests)
                {
                    var response = await server.SendAsync(context =>
                    {
                        context.Request.Path = test.endpoint;
                        context.User = principal;
                    });

                    Assert.Equal(test.code, response.Response.StatusCode);
                }

            _timeServiceMock.Reset();
        }

        [Fact]
        public async Task TestTwoTimesPerSecondForIpStrictionRegexPolicy()
        {
            _timeServiceMock.Setup(t => t.GetUTC()).Returns(_baseTime);
            var server = _factory.Server;

            var tests = new[]
            {
                (code : StatusCodes.Status200OK, endpoint: "/api/raw/ipfunction"),
                (code : StatusCodes.Status200OK, endpoint: "/api/raw/functionip"),
                (code : StatusCodes.Status429TooManyRequests, endpoint: "/api/raw/ipfunction"),
                (code : StatusCodes.Status429TooManyRequests, endpoint: "/api/raw/functionip")
            };

            var ipV4Array = new[] { IPAddress.Parse("127.168.1.31"), IPAddress.Parse("127.168.1.32") };

            foreach (var ipV4 in ipV4Array)
                foreach (var test in tests)
                {
                    var response = await server.SendAsync(context =>
                    {
                        context.Request.Path = test.endpoint;
                        context.Request.Method = HttpMethods.Get;
                        context.Connection.RemoteIpAddress = ipV4;
                    });

                    Assert.Equal(test.code, response.Response.StatusCode);
                }

            _timeServiceMock.Reset();
        }

        [Fact]
        public async Task TestTwoTimesPerSecondForEndpointStrictionRegexPolicy()
        {
            _timeServiceMock.Setup(t => t.GetUTC()).Returns(_baseTime);
            using var client = _factory.CreateClient();

            var tests = new[]
            {
                (code : HttpStatusCode.OK, endpoint: "/api/raw/endpointfunction"),
                (code : HttpStatusCode.OK, endpoint: "/api/raw/functionendpoint"),
                (code : HttpStatusCode.TooManyRequests, endpoint: "/api/raw/endpointfunction"),
                (code : HttpStatusCode.TooManyRequests, endpoint: "/api/raw/functionendpoint")
            };

            foreach (var test in tests)
            {
                var response = await client.GetAsync(test.endpoint);

                Assert.Equal(test.code, response.StatusCode);
            }

            _timeServiceMock.Reset();
        }
    }
}
