using ManageRates.AspnetCore.Abstractions;
using ManageRates.AspnetCore.Policies;
using ManageRates.Core;
using ManageRates.Core.Abstractions;
using ManageRates.Core.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;

namespace ManageRates.AspnetCore.Builder
{
    /// <summary>
    /// Extension methods to add rate strictions capabilities to an HTTP application pipeline.
    /// </summary>
    public static class ManageRatesEndpointRouteBuilderExtensions
    {

        public static TBuilder ManageRatesByIp<TBuilder>(this TBuilder builder, int count, RatesStrictPeriod period)
            where TBuilder : IEndpointConventionBuilder
        {
            IHttpContextRatePolicy policy = new IpManageRatePolicy(count, period);
            builder.Add(endpointBuilder => endpointBuilder.Metadata.Add(policy));

            return builder;
        }

        public static TBuilder ManageRatesByUser<TBuilder>(this TBuilder builder, int count, RatesStrictPeriod period)
            where TBuilder : IEndpointConventionBuilder
        {
            IHttpContextRatePolicy policy = new UserManageRatePolicy(count, period);
            builder.Add(endpointBuilder => endpointBuilder.Metadata.Add(policy));

            return builder;
        }

        public static TBuilder ManageRates<TBuilder>(this TBuilder builder, int count, RatesStrictPeriod period)
            where TBuilder : IEndpointConventionBuilder
        {
            IHttpContextRatePolicy policy = new EnpointManageRatePolicy(count, period);
            builder.Add(endpointBuilder => endpointBuilder.Metadata.Add(policy));

            return builder;
        }

        public static TBuilder ManageRatesByDelegate<TBuilder>(this TBuilder builder, Func<HttpContext, ITimeService, ManageRatesResult> delegatePolicy)
            where TBuilder : IEndpointConventionBuilder
        {
            IHttpContextRatePolicy policy = new DelegateManageRatePolicy(delegatePolicy);
            builder.Add(endpointBuilder => endpointBuilder.Metadata.Add(policy));

            return builder;
        }
    }
}
