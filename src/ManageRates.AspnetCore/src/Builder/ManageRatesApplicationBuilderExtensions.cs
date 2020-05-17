using ManageRates.AspnetCore;
using ManageRates.AspnetCore.Abstractions;
using ManageRates.Core;
using ManageRates.Core.Abstractions;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to add rate manage capabilities to DI.
    /// </summary>
    public static class ManageRatesApplicationBuilderExtensions
    {
        /// <summary>
        /// Add required services to <see cref="IServiceCollection"/>.
        /// </summary>
        public static void AddRateStrictions(this IServiceCollection services)
        {
            services.AddSingleton<ITimeService, TimeService>();
            services.AddSingleton<IManageRatesService, ManageRatesService>();
        }
    }
}
