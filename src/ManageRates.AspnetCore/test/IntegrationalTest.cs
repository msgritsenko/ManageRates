using ManageRates.AspnetCore.Builder;
using ManageRates.Core;
using ManageRates.Core.Abstractions;
using ManageRates.Core.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace ManageRates.AspnetCore.Tests
{
    /// <summary>
    /// The test server wich will be used in all tests.
    /// </summary>
    /// <remarks>
    /// Each policy has 2 times in one second.
    /// </remarks>
    public class IntegrationalTestHost : IDisposable
    {
        public IHost Host { get; }
        public Mock<ITimeService> TimeServiceMock { get; }
        public DateTime BaseTime { get; }

        public IntegrationalTestHost()
        {
            TimeServiceMock = new Mock<ITimeService>();
            BaseTime = new DateTime(2020, 01, 01);
            TimeServiceMock.Setup(t => t.GetUTC()).Returns(BaseTime);

            Host = new HostBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddRateStrictions();
                    services.AddSingleton<ITimeService>(TimeServiceMock.Object);
                    services.AddRouting();
                })
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder
                        .UseTestServer()
                        .Configure(app =>
                        {
                            app.UseRouting();
                            app.UseMiddleware<ManageRatesMiddleware>();
                            app.UseEndpoints(endpoints =>
                            {
                                endpoints.MapGet("/endpoint", context => context.Response.WriteAsync("endpoint"))
                                    .ManageRates(2, RatesStrictPeriod.Second);

                                endpoints.MapGet("/user", context => context.Response.WriteAsync("user"))
                                    .ManageRatesByUser(2, RatesStrictPeriod.Second);

                                endpoints.MapGet("/ip", context => context.Response.WriteAsync("ip"))
                                    .ManageRatesByIp(2, RatesStrictPeriod.Second);

                                endpoints.MapGet("/delegate", context => context.Response.WriteAsync("delegate"))
                                    .ManageRates((c, t) => new ManageRatesResult(false));
                            });
                        });
                })
                .Start();

            Host.GetTestServer().BaseAddress = new Uri("http://localhost:5000");
        }

        public void Dispose()
        {
            Host?.Dispose();
        }
    }

    /// <summary>
    /// Need to ensure that union test run will be run the last caused time shifting of server.
    /// </summary>
    public class IntegrationalTestOrderer : ITestCaseOrderer
    {
        public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases)
                where TTestCase : ITestCase
        {
            var result = testCases.ToList();

            var lastResult = result.Single(x => x.TestMethod.Method.Name.Equals(nameof(IntegrationalTest.AllTestsInParallel)));

            result.Remove(lastResult);
            result.Add(lastResult);

            return result;
        }
    }


    [TestCaseOrderer("ManageRates.AspnetCore.Tests.IntegrationalTestOrderer", "ManageRates.AspnetCore.Tests")]
    public class IntegrationalTest : IClassFixture<IntegrationalTestHost>
    {
        private readonly IntegrationalTestHost _host;

        public IntegrationalTest(IntegrationalTestHost host)
        {
            _host = host;
        }

        /// <summary>
        /// Runs all tests two times with time shift to unlock resources. Should be run the last.
        /// </summary>
        [Fact]
        public async Task AllTestsInParallel()
        {
            _host.TimeServiceMock.Setup(t => t.GetUTC()).Returns(_host.BaseTime.AddHours(1));

            await Task.WhenAll(
                Task.Run(TestEndpointRate),
                Task.Run(TestIpRateForTwoDifferentUsers),
                Task.Run(TestIpRateForTwoDifferentIP),
                Task.Run(TestDelegateRate));

            
            _host.TimeServiceMock.Setup(t => t.GetUTC()).Returns(_host.BaseTime.AddHours(2));

            await Task.WhenAll(
                Task.Run(TestEndpointRate),
                Task.Run(TestIpRateForTwoDifferentUsers),
                Task.Run(TestIpRateForTwoDifferentIP),
                Task.Run(TestDelegateRate));
        }

        [Fact]
        public async Task TestEndpointRate()
        {
            using var client = _host.Host.GetTestClient();

            var answers = new[]
            {
                HttpStatusCode.OK,
                HttpStatusCode.OK,
                HttpStatusCode.TooManyRequests,
                HttpStatusCode.TooManyRequests
            };

            foreach (var answer in answers)
            {
                var okResponse = await client.GetAsync("/endpoint");

                Assert.Equal(answer, okResponse.StatusCode);
            }
        }

        [Fact]
        public async Task TestIpRateForTwoDifferentUsers()
        {
            TestServer server = _host.Host.GetTestServer();
            
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
                        context.Request.Path = "/user";
                        context.User = principal;
                    });

                    Assert.Equal(answer, okResponse.Response.StatusCode);
                }
        }


        [Fact]
        public async Task TestIpRateForTwoDifferentIP()
        {
            TestServer server = _host.Host.GetTestServer();
            
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
                        context.Request.Path = "/ip";
                        context.Request.Method = HttpMethods.Get;
                        context.Connection.RemoteIpAddress = ipV4;
                    });

                    Assert.Equal(answer, okResponse.Response.StatusCode);
                }
        }

        [Fact]
        public async Task TestDelegateRate()
        {
            using var client = _host.Host.GetTestClient();

            for (int i = 0; i < 10; ++i)
            {
                var response = await client.GetAsync("/delegate");
                Assert.Equal(HttpStatusCode.TooManyRequests, response.StatusCode);
            }
        }
    }
}
