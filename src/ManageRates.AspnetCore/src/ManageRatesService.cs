using ManageRates.AspnetCore.Abstractions;
using ManageRates.AspnetCore.Configuration;
using ManageRates.Core.Abstractions;
using ManageRates.Core.Model;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace ManageRates.AspnetCore
{
    /// <inheritdoc/>
    public class ManageRatesService : IManageRatesService
    {
        private readonly ITimeService _timeService;

        /// <summary>
        /// Creates new instance <see cref="ManageRatesService"/>.
        /// </summary>
        public ManageRatesService(ITimeService timeService)
        {
            _timeService = timeService;
        }

        /// <inheritdoc/>
        public ManageRatesResult Process(HttpContext context, ManageRatesConfiguration policies = null)
        {
            var endpoint = context.GetEndpoint();

            var policy = endpoint?.Metadata.GetMetadata<IHttpContextRatePolicy>();
            if (policy != null)
            {
                return policy.IsPermitted(context, _timeService);
            }

            policy = policies?.Policies.FirstOrDefault(p => p.Accept(context));
            if (policy != null)
                return policy.IsPermitted(context, _timeService);

            return new ManageRatesResult(true);
        }

    }
}