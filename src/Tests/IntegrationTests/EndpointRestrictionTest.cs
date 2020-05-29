using ManageRates.Core.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Integration.Tests
{

    public class EndpointRestrictionTest : IClassFixture<WebApplicationFactory<WebApi.Startup>>
    {
        private readonly DateTime _baseTime;
        private readonly Mock<ITimeService> _timeServiceMock;
        private readonly WebApplicationFactory<WebApi.Startup> _factory;

        public EndpointRestrictionTest(WebApplicationFactory<WebApi.Startup> factory)
        {
            _baseTime = new DateTime(2020, 01, 01);
            _timeServiceMock = new Mock<ITimeService>();

            _factory = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        //services.AddSingleton(_timeServiceMock.Object);
                    });
                });
        }

        [Fact]
        public async Task TestEndpointHeavyLoadStricts()
        {
            int response200 = 0;
            int response429 = 0;

            Action<HttpStatusCode> registerResult = code =>
            {
                if (code == HttpStatusCode.OK)
                    Interlocked.Increment(ref response200);

                if (code == HttpStatusCode.TooManyRequests)
                    Interlocked.Increment(ref response429);
            };

            var seconds = 10;
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(seconds));

            // start thread to managed resource
            var tasks = Enumerable.Range(0, 2)
                .Select(i => TestEndpoint("/endpoint", cts.Token, registerResult))
                .ToList();

            await Task.WhenAll(tasks);

            //Assert.Equal(seconds * 2, response200);
        }

        private async Task TestEndpoint(
            string endpoint,
            CancellationToken cancellationToken,
            Action<HttpStatusCode> registerResult)
        {
            await Task.Yield();

            using var client = _factory.CreateClient();

            while (!cancellationToken.IsCancellationRequested)
            {
                var response = await client.GetAsync(endpoint);
                registerResult(response.StatusCode);
                await Task.Delay(240);
            }
        }
    }
}
