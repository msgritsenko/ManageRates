using ManageRates.AspnetCore.Abstractions;
using ManageRates.Core.Abstractions;
using ManageRates.Core.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace ManageRates.AspnetCore.Infrastructure
{
    /// <summary>
    /// Special User rate striction attribute.
    /// </summary>
    /// <remarks>Just only as a short version of <see cref="EndpointManageRateAttribute"/>.</remarks>
    public class ManageRateAttribute : Attribute, IHttpManageRatePolicy
    {
        private readonly IHttpManageRatePolicy _policy;

        public ManageRateAttribute(int count, RatesStrictPeriod period, RatesStricType strictType)
        {
            _policy = HttpManageRatePolicyBuilder.Build(count, period, strictType);
        }

        /// <inheritdoc/>
        public bool Accept(HttpContext context) => _policy.Accept(context);

        /// <inheritdoc/>
        public ManageRatesResult IsPermitted(HttpContext context, ITimeService timeService, IMemoryCache memoryCache)
        {
            return _policy.IsPermitted(context, timeService, memoryCache);
        }
    }
}
