using ManageRates.AspnetCore.Abstractions;
using ManageRates.Core;
using ManageRates.Core.Abstractions;
using ManageRates.Core.Model;
using Microsoft.AspNetCore.Http;
using System;

namespace ManageRates.AspnetCore.Policies
{
    /// <summary>
    /// Implementations of <see cref="IHttpContextRatePolicy"/> wich policy resolver binded to specific <see cref="Endpoint"/>.
    /// </summary>
    public class EnpointManageRatePolicy : IHttpContextRatePolicy
    {
        private readonly IManageRatePolicy _policy;

        public EnpointManageRatePolicy(int count, RatesStrictPeriod period)
        {
            _policy = ManageRateBulder.BuildDefaultPolicy(period, count);
        }

        /// <inheritdoc/>
        public bool Accept(HttpContext context)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ManageRatesResult IsPermitted(HttpContext context, ITimeService timeService)
        {
            var permitted = _policy.IsPermitted(timeService);

            return new ManageRatesResult(permitted);
        }
    }
}
