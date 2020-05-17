using ManageRates.AspnetCore.Abstractions;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace ManageRates.AspnetCore
{
    public class ManageRatesMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IManageRatesService _manageRatesService;

        public ManageRatesMiddleware(
            RequestDelegate next,
            IManageRatesService manageRatesService)
        {
            _next = next;
            _manageRatesService = manageRatesService ?? throw new ArgumentNullException(nameof(manageRatesService));
        }

        public Task InvokeAsync(HttpContext httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            var result = _manageRatesService.Process(httpContext);

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
