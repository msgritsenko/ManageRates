using ManageRates.AspnetCore.Abstractions;
using ManageRates.Core.Model;
using Microsoft.AspNetCore.Builder;
using static ManageRates.AspnetCore.HttpManageRatePolicy;

namespace ManageRates.AspnetCore.Builder
{
    /// <summary>
    /// Extension methods to add rate strictions capabilities to an HTTP application pipeline.
    /// </summary>
    public static class ManageRatesEndpointRouteBuilderExtensions
    {

        public static TBuilder ManageRates<TBuilder>(
            this TBuilder builder,
            int count,
            RatesStrictPeriod period,
            RatesStricType strictType) where TBuilder : IEndpointConventionBuilder
        {
            IHttpManageRatePolicy policy = HttpManageRatePolicyBuilder.Build(count, period, strictType);
            builder.Add(endpointBuilder => endpointBuilder.Metadata.Add(policy));

            return builder;
        }

        public static TBuilder ManageRates<TBuilder>(
            this TBuilder builder,
            PolicyDelegate policyDelegate) where TBuilder : IEndpointConventionBuilder
        {
            IHttpManageRatePolicy policy = HttpManageRatePolicyBuilder.Build(policyDelegate);
            builder.Add(endpointBuilder => endpointBuilder.Metadata.Add(policy));

            return builder;
        }
    }
}
