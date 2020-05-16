using ManageRates.AspnetCore.Abstractions;
using ManageRates.Core;
using ManageRates.Core.Abstractions;
using ManageRates.Core.Model;
using Microsoft.AspNetCore.Http;
using System;

namespace ManageRates.AspnetCore.Policies
{
    /// <summary>
    /// Implementations of <see cref="IHttpContextRatePolicy"/> with User-buinded policy resolver.
    /// </summary>
    class UserManageRatePolicy : IHttpContextRatePolicy
    {
        private readonly IKeyedManageRatePolicy _policy;

        public UserManageRatePolicy(int count, RatesStrictPeriod period)
        {
            _policy = ManageRateBulder.BuildKeyedDefaultPolicy(period, count);
        }

        /// <inheritdoc/>
        public bool Accept(HttpContext context)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ManageRatesResult IsPermitted(HttpContext context, ITimeService timeService)
        {
            string user = context.User?.Identity.Name;
            if (string.IsNullOrEmpty(user))
                return new ManageRatesResult(false);

            var permitted = _policy.IsPermitted(user, timeService);

            return new ManageRatesResult(permitted);
        }
    }
}
