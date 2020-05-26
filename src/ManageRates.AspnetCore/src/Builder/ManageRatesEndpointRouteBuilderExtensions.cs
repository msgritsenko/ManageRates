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
            Period period,
            KeyType keyType) where TBuilder : IEndpointConventionBuilder
        {
            IHttpManageRatePolicy policy = HttpManageRatePolicyBuilder.Build(count, period, keyType);
            builder.Add(endpointBuilder => endpointBuilder.Metadata.Add(policy));

            return builder;
        }

        public static TBuilder ManageRates<TBuilder>(
            this TBuilder builder,
            string policyName) where TBuilder : IEndpointConventionBuilder
        {
            IHttpManageRatePolicy policy = HttpManageRatePolicyBuilder.Build(policyName);
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
