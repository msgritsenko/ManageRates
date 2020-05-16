using ManageRates.AspnetCore.Abstractions;
using ManageRates.Core.Abstractions;
using ManageRates.Core.Model;
using Microsoft.AspNetCore.Http;

namespace ManageRates.AspnetCore
{
    /// <summary>
    /// Service for mana
    /// </summary>
    public class ManageRatesService 
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
        public ManageRatesResult Process(HttpContext context)
        {
            var endpoint = context.GetEndpoint();

            var policy = endpoint?.Metadata.GetMetadata<IHttpContextRatePolicy>();
            if (policy != null)
            {
                return policy.IsPermitted(context, _timeService);
            }

            return new ManageRatesResult(true);
        }
    }
}