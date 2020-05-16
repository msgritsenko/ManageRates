using ManageRates.AspnetCore.Abstractions;
using ManageRates.Core;
using ManageRates.Core.Abstractions;
using ManageRates.Core.Model;
using Microsoft.AspNetCore.Http;
using System;

namespace ManageRates.AspnetCore.Policies
{
    /// <summary>
    /// Implementations of <see cref="IHttpContextRatePolicy"/> with IP-binded polocy resolver.
    /// </summary>
    public class IpManageRatePolicy : IHttpContextRatePolicy
    {
        private readonly IKeyedManageRatePolicy _policy;

        public IpManageRatePolicy(int count, RatesStrictPeriod period)
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
            string ip = context.Connection.RemoteIpAddress.ToString();
            var permitted = _policy.IsPermitted(ip, timeService);

            return new ManageRatesResult(permitted);
        }
    }
}
