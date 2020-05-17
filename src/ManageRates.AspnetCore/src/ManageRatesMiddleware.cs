using ManageRates.AspnetCore.Abstractions;
using ManageRates.AspnetCore.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace ManageRates.AspnetCore
{
    public class ManageRatesMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IOptions<ManageRatesConfiguration> _manageRatePolicies;
        private readonly IManageRatesService _manageRatesService;

        public ManageRatesMiddleware(
            RequestDelegate next,
            IOptions<ManageRatesConfiguration> manageRatesPolicies,
            IManageRatesService manageRatesService)
        {
            _next = next;
            _manageRatePolicies = manageRatesPolicies;
            _manageRatesService = manageRatesService ?? throw new ArgumentNullException(nameof(manageRatesService));
        }

        public Task InvokeAsync(HttpContext httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            var result = _manageRatesService.Process(httpContext, _manageRatePolicies.Value);

            if (result.Permitted)
                return _next(httpContext);
            else
            {
                httpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                httpContext.Response.ContentType = "text/plain";
                return httpContext.Response.WriteAsync("Rate limit exceeded");
            }
        }
    }
}
