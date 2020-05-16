using ManageRates.AspnetCore.Abstractions;
using ManageRates.Core.Abstractions;
using ManageRates.Core.Model;
using Microsoft.AspNetCore.Http;
using System;

namespace ManageRates.AspnetCore.Policies
{
    /// <summary>
    /// Implementations of <see cref="IHttpContextRatePolicy"/> with delegate policy resolver.
    /// </summary>
    public class DelegateManageRatePolicy : IHttpContextRatePolicy
    {
        private readonly Func<HttpContext, ITimeService, ManageRatesResult> _policy;

        public DelegateManageRatePolicy(Func<HttpContext, ITimeService, ManageRatesResult> policy)
        {
            _policy = policy;
        }

        /// <inheritdoc/>
        public bool Accept(HttpContext context)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ManageRatesResult IsPermitted(HttpContext context, ITimeService timeService)
        {
            return _policy(context, timeService);
        }
    }
}
