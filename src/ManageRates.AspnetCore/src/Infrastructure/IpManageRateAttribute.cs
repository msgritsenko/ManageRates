using ManageRates.AspnetCore.Abstractions;
using ManageRates.AspnetCore.Policies;
using ManageRates.Core.Abstractions;
using ManageRates.Core.Model;
using Microsoft.AspNetCore.Http;
using System;

namespace ManageRates.Core
{
    /// <summary>
    /// Special IP rate striction attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class IpManageRateAttribute : Attribute, IHttpContextRatePolicy
    {
        private readonly IpManageRatePolicy _policy;

        public IpManageRateAttribute(int count, RatesStrictPeriod period)
        {
            _policy = new IpManageRatePolicy(count, period);
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
