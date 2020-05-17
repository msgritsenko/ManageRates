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
        public static IApplicationBuilder UseManageRates(this IApplicationBuilder app)
        {
            return app.UseManageRates(b => { });
        }

        public static IApplicationBuilder UseManageRates(this IApplicationBuilder app, Action<ManageRatesConfigurationBuilder> policyBuilder)
        {
            if (app.ApplicationServices.GetService(typeof(IManageRatesService)) == null)
                throw new InvalidOperationException($"Unable to finde service {nameof(IManageRatesService)}");

            var manageRatesPolicyBuilder = new ManageRatesConfigurationBuilder();

            policyBuilder(manageRatesPolicyBuilder);
            ManageRatesConfiguration policies = manageRatesPolicyBuilder.Build();
            var options = Microsoft.Extensions.Options.Options.Create(policies);

            app.UseMiddleware<ManageRatesMiddleware>(options);
            return app;
        }
    }
}
