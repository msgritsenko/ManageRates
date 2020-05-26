using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Threading.Tasks;
using Xunit;

namespace Load.Tests
{

    /// <summary>
    /// Checks if there is memory leak with <see cref="QUERY_COUNT"/> queries and <see cref="THREAD_COUNT"/>.
    /// Memory limit is <see cref="MEMORY_LIMIT"/>.
    /// </summary>
    public class MemoryLeakTest : IClassFixture<WebApplicationFactory<WebApi.Startup>>
    {
        public const int QUERY_COUNT = 100_000_000;
        public const int THREAD_COUNT = 10;
        public const int MEMORY_LIMIT = 300 * 1024 * 1024;

        delegate Task<int> testDelegate(string endpoint);

        private readonly WebApplicationFactory<WebApi.Startup> _factory;
        private readonly Random _random;

        private readonly IReadOnlyDictionary<string, testDelegate> _testEndpoints;

        public MemoryLeakTest(WebApplicationFactory<WebApi.Startup> factory)
        {
            _factory = factory
                .WithWebHostBuilder(builder =>
                {
                });

            _random = new Random();

            _testEndpoints = new Dictionary<string, testDelegate>
            {
                { "/user", TestWithRandomUser},
                { "/MethodAttributes/user", TestWithRandomUser},

                { "/ip", TestWithRandomIp },
                { "/MethodAttributes/ip", TestWithRandomIp },

                { "/ControllerAttribute", TestEndpoint },
                { "/MethodAttributes/endpoint", TestEndpoint },
                { "/endpoint", TestEndpoint },
            };
        }

        [Trait("Category", "LongRunning")]
        [Fact]
        public void TestWith10Threads_1hour_CheckMaxMemory()
        {
            var endpoints = _testEndpoints.Keys.ToList();
            int maxIndex = endpoints.Count;
            var options = new ParallelOptions()
            {
                MaxDegreeOfParallelism = THREAD_COUNT
            };

            Parallel.For(1, QUERY_COUNT, options, i =>
            {
                if (i % 10 == 0)
                {
                    var memory = GC.GetTotalMemory(false);
                    Assert.True(memory < MEMORY_LIMIT);
                }

                int index = _random.Next(maxIndex);

                var endpoint = endpoints[index];
                var func = _testEndpoints[endpoint];

                var responseCode = func.Invoke(endpoint).Result;

                bool supportedResult = responseCode == StatusCodes.Status200OK ||
                                        responseCode == StatusCodes.Status429TooManyRequests;

                Assert.True(supportedResult);
            });
        }


        private async Task<int> TestWithRandomUser(string endpoint)
        {
            var userId = _random.Next();
            var principal = new GenericPrincipal(new GenericIdentity($"User_{userId}"), null);
            var server = _factory.Server;

            var response = await server.SendAsync(context =>
            {
                context.Request.Path = endpoint;
                context.User = principal;
            });

            return response.Response.StatusCode;
        }


        private async Task<int> TestWithRandomIp(string endpoint)
        {
            var ip = IPAddress.Parse($"{_random.Next(1, 255)}.{_random.Next(1, 255)}.{_random.Next(1, 255)}.{_random.Next(1, 255)}");
            var server = _factory.Server;

            var response = await server.SendAsync(context =>
            {
                context.Request.Path = endpoint;
                context.Request.Method = HttpMethods.Get;
                context.Connection.RemoteIpAddress = ip;
            });

            return response.Response.StatusCode;
        }

        private async Task<int> TestEndpoint(string endpoint)
        {
            using (var client = _factory.CreateClient())
            {
                var response = await client.GetAsync(endpoint);
                return (int)response.StatusCode;
            }
        }
    }
}
