using ManageRates.AspnetCore.Abstractions;
using ManageRates.AspnetCore.Configuration;
using ManageRates.Core.Abstractions;
using ManageRates.Core.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;

namespace ManageRates.AspnetCore
{
    /// <inheritdoc/>
    public class ManageRatesService : IManageRatesService
    {
        private readonly ITimeService _timeService;
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// Creates new instance <see cref="ManageRatesService"/>.
        /// </summary>
        public ManageRatesService(ITimeService timeService, IMemoryCache memoryCache)
        {
            _timeService = timeService ?? throw new ArgumentNullException(nameof(timeService));
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        /// <inheritdoc/>
        public ManageRatesResult Process(HttpContext context, ManageRatesConfiguration policies = null)
        {
            var endpoint = context.GetEndpoint();

            var policy = endpoint?.Metadata.GetMetadata<IHttpManageRatePolicy>();

            if (policy != null)
            {
                if (!string.IsNullOrEmpty(policy.ReferenceName))
                    policy = policies?.Policies?.FirstOrDefault(p => policy.ReferenceName.Equals(p.Name));

                if (policy != null)
                    return policy.IsPermitted(context, _timeService, _memoryCache);
            }

            policy = policies?.Policies.FirstOrDefault(p => p.Accept(context));
            if (policy != null)
                return policy.IsPermitted(context, _timeService, _memoryCache);

            return new ManageRatesResult(true);
        }

    }
}