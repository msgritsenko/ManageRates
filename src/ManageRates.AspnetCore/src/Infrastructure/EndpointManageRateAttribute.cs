using ManageRates.AspnetCore.Abstractions;
using ManageRates.AspnetCore.Policies;
using ManageRates.Core.Abstractions;
using ManageRates.Core.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace ManageRates.Core
{
    /// <summary>
    /// Common rate striction attribute regardless of user o IP.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class EndpointManageRateAttribute : Attribute, IHttpContextRatePolicy
    {
        private readonly EndpointManageRatePolicy _policy;

        public EndpointManageRateAttribute(int count, RatesStrictPeriod period)
        {
            _policy = new EndpointManageRatePolicy(count, period);
        }

        /// <inheritdoc/>
        public bool Accept(HttpContext context)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ManageRatesResult IsPermitted(HttpContext context, ITimeService timeService, IMemoryCache memoryCache)
        {
            return _policy.IsPermitted(context, timeService, memoryCache);
        }
    }
}
