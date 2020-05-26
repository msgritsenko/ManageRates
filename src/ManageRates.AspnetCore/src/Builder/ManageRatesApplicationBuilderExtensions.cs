using ManageRates.AspnetCore;
using ManageRates.AspnetCore.Abstractions;
using ManageRates.AspnetCore.Configuration;
using Microsoft.AspNetCore.Builder;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// <see cref="IApplicationBuilder"/> extension methods for the <see cref="ManageRatesMiddleware"/>.
    /// </summary>
    public static class ManageRatesApplicationBuilderExtensions
    {
        /// <summary>
        /// Add manage rates middleware.
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseManageRates(this IApplicationBuilder app)
        {
            return app.UseManageRates(b => { });
        }


        /// <summary>
        /// Add manage rates middleware and setup set of manage rate policies.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="policyBuilder">Action of <see cref="ManageRatesConfigurationBuilder"/> to configure set of policies.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseManageRates(this IApplicationBuilder app, Action<ManageRatesConfigurationBuilder> policyBuilder)
        {
            if (app.ApplicationServices.GetService(typeof(IManageRatesService)) == null)
                throw new InvalidOperationException($"Unable to finde service {nameof(IManageRatesService)}");

            var manageRatesPolicyBuilder = new ManageRatesConfigurationBuilder();

            policyBuilder(manageRatesPolicyBuilder);
            ManageRatesConfiguration policies = manageRatesPolicyBuilder.Build();
            var options = Options.Options.Create(policies);

            app.UseMiddleware<ManageRatesMiddleware>(options);
            return app;
        }
    }
}
