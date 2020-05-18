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
    /// Special User rate striction attribute.
    /// </summary>
    /// <remarks>Just only as a short version of <see cref="EndpointManageRateAttribute"/>.</remarks>
    public class UserManageRateAttribute : Attribute, IHttpContextRatePolicy
    {
        private readonly UserManageRatePolicy _policy;

        public UserManageRateAttribute(int count, RatesStrictPeriod period)
        {
            _policy = new UserManageRatePolicy(count, period);
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
