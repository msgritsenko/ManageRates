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
    public class ManageRateAttribute : Attribute, IHttpManageRatePolicy
    {
        /// <summary>
        /// Of course we don't implement policy logic in each component, we will use stardard.
        /// </summary>
        private readonly IHttpManageRatePolicy _policy;

        /// <summary>
        /// Creates individual policy with particular charecteristics.
        /// </summary>
        public ManageRateAttribute(int count, Period period, KeyType keyType)
        {
            _policy = HttpManageRatePolicyBuilder.Build(count, period, keyType);
        }

        /// <summary>
        /// Create attribute contains reference to named policy.
        /// </summary>
        /// <param name="policyName">Name of the named policy.</param>
        public ManageRateAttribute(string policyName)
        {
            ReferenceName = policyName;
        }

        public string Name { get; set; }

        public string ReferenceName { get; }

        /// <inheritdoc/>
        public bool Accept(HttpContext context) => _policy.Accept(context);

        /// <inheritdoc/>
        public ManageRatesResult IsPermitted(HttpContext context, ITimeService timeService, IMemoryCache memoryCache)
        {
            return _policy.IsPermitted(context, timeService, memoryCache);
        }
    }
}
