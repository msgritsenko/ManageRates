using ManageRates.AspnetCore;
using ManageRates.Core;
using Microsoft.AspNetCore.Builder;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ManageRatesApplicationBuilderExtensions
    {
        public static void AddRateStrictions(this IServiceCollection services)
        {
            services.AddSingleton<ManageRatesService>();
            services.AddSingleton<ManageRatesMiddleware>();
        }

        public static void UseRateStrictions(this IApplicationBuilder app)
        {
            app.UseRateStrictions(configuration => { });
        }

        public static void UseRateStrictions(this IApplicationBuilder app, Action<ManageRatesConfiguration> configureConfiguration)
        {
            var configuration = new ManageRatesConfiguration();
            configureConfiguration(configuration);

            app.UseMiddleware<ManageRatesMiddleware>(configuration);
        }
    }
}
