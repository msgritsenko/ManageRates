using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ManageRates.AspnetCore.Tests
{
    public class ManageRatesMiddlewareTests
    {
        private const int HighLoadRequestsCount = 100_000;

        [Fact]
        public async Task SequentialHighLoadTest_EmptyMiddleware_ReturnsOk()
        {
            using var host = await CreateRawTestServer();
            using var client = host.GetTestClient();

            for (int i = 0; i < HighLoadRequestsCount; ++i)
            {
                var response = await client.GetAsync("/");
                response.EnsureSuccessStatusCode();

                var answer = await response.Content.ReadAsStringAsync();
                Assert.Equal("ok", answer, true);
            }
        }

        [Fact]
        public async Task ParallelHighLoadTest_EmptyMiddleware_ReturnsOk()
        {
            using var host = await CreateRawTestServer();
            using var client = host.GetTestClient();

            int responseCount = 0;

            for (int i = 0; i < HighLoadRequestsCount; ++i)
            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                Task.Run(() =>
                {
                    var response = client.GetAsync("/");
                    response.ContinueWith(task =>
                    {
                        Interlocked.Increment(ref responseCount);
                    });
                });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }

            // wait while all tasks finished
            var timeWait = TimeSpan.FromMilliseconds(100);
            while (responseCount < HighLoadRequestsCount)
            {
                await Task.Delay(timeWait);
            }
        }

        /// <summary>
        /// Test server without any middleware in pipeline.
        /// </summary>
        /// <returns></returns>
        private static Task<IHost> CreateRawTestServer()
        {
            return CreateTestHost(
                services => { },
                app =>
                    app.Run(async context =>
                    {
                        await context.Response.WriteAsync("ok");
                    })
            );
        }


        /// <summary>
        /// Most common part of configuration of the test Service.
        /// </summary>
        /// <param name="configureServices">Action to configure DI services.</param>
        /// <param name="configureApp">Action to configure application pipeline.</param>
        /// <returns></returns>
        private static Task<IHost> CreateTestHost(
            Action<IServiceCollection> configureServices,
            Action<IApplicationBuilder> configureApp)
        {
            var host = new HostBuilder()
                .ConfigureServices((context, services) =>
                {
                    configureServices(services);
                })
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder
                        .UseTestServer()
                        .Configure(app =>
                        {
                            configureApp(app);

                        });
                })
                .StartAsync();

            return host;
        }
    }
}