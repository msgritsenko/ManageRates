using ManageRates.AspnetCore.Abstractions;
using ManageRates.Core.Abstractions;
using ManageRates.Core.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace ManageRates.AspnetCore
{
    /// <summary>
    /// Implementations of <see cref="IHttpManageRatePolicy"/> with delegate policy resolver.
    /// </summary>
    public class HttpManageRatePolicy : IHttpManageRatePolicy
    {
        public delegate ManageRatesResult PolicyDelegate(HttpContext httpContext, ITimeService timeService, IMemoryCache memoryCache);
        public delegate bool AcceptDelegate(HttpContext httpContext);

        private readonly PolicyDelegate _policy;
        private readonly AcceptDelegate _accept;

        public string Name { get; set; }
        public string ReferenceName { get; set; }

        public HttpManageRatePolicy(PolicyDelegate policy)
        {
            _policy = policy;
        }
        public HttpManageRatePolicy(PolicyDelegate policy, AcceptDelegate accept)
        {
            _policy = policy;
            _accept = accept;
        }

        /// <inheritdoc/>
        public bool Accept(HttpContext context)
        {
            if (_accept == null)
                throw new NotImplementedException();

            return _accept(context);
        }

        /// <inheritdoc/>
        public ManageRatesResult IsPermitted(HttpContext context, ITimeService timeService, IMemoryCache memoryCache)
        {
            return _policy(context, timeService, memoryCache);
        }
    }
}
