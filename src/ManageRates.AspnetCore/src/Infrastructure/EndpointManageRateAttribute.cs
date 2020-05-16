using ManageRates.AspnetCore.Abstractions;
using ManageRates.AspnetCore.Policies;
using ManageRates.Core.Abstractions;
using ManageRates.Core.Model;
using Microsoft.AspNetCore.Http;
using System;

namespace ManageRates.Core
{
    /// <summary>
    /// Common rate striction attribute regardless of user o IP.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class EndpointManageRateAttribute : Attribute, IHttpContextRatePolicy
    {
        private readonly EnpointManageRatePolicy _policy;

        public EndpointManageRateAttribute(int count, RatesStrictPeriod period)
        {
            _policy = new EnpointManageRatePolicy(count, period);
        }

        /// <inheritdoc/>
        public bool Accept(HttpContext context)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ManageRatesResult IsPermitted(HttpContext context, ITimeService timeService)
        {
            return _policy.IsPermitted(context, timeService);
        }
    }
}
