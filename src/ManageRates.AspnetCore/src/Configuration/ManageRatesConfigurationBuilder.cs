using ManageRates.AspnetCore.Abstractions;
using ManageRates.Core.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using static ManageRates.AspnetCore.HttpManageRatePolicy;

namespace ManageRates.AspnetCore.Configuration
{
    public class ManageRatesConfigurationBuilder
    {
        private readonly List<IHttpManageRatePolicy> _policies;
        public ManageRatesConfigurationBuilder()
        {
            _policies = new List<IHttpManageRatePolicy>();
        }

        /// <summary>
        /// Add policy witch catches request with request patrh meets <paramref name="endpointPathPattern"/> of <see cref="Regex"/>.
        /// </summary>
        /// <param name="endpointPathPattern">Regex pattern with <see cref="RegexOptions.IgnoreCase"/> and <see cref="RegexOptions.Compiled"/> options.</param>
        /// <param name="count">Allowed rate count in <paramref name="period"/></param>
        /// <param name="period">Restriction period.</param>
        /// <param name="stricType"><see cref="KeyType"/> to distinguish rates.</param>
        /// <returns></returns>
        public ManageRatesConfigurationBuilder AddPolicy(string endpointPathPattern, int count, Period period, KeyType stricType)
        {
            var policy = HttpManageRatePolicyBuilder.Build(endpointPathPattern, count, period, stricType);
            _policies.Add(policy);

            return this;
        }

        public ManageRatesConfigurationBuilder AddNamedPolicy(string policyName, string endpointPathPattern, int count, Period period, KeyType stricType)
        {
            var policy = HttpManageRatePolicyBuilder.Build(endpointPathPattern, count, period, stricType)
                .WithName(policyName);
            _policies.Add(policy);

            return this;
        }

        public ManageRatesConfigurationBuilder AddNamedPolicy(string policyName, int count, Period period, KeyType stricType)
        {
            var policy = HttpManageRatePolicyBuilder.Build(".*", count, period, stricType)
                .WithName(policyName);
            _policies.Add(policy);

            return this;
        }

        public ManageRatesConfigurationBuilder AddNamedPolicy(string policyName, int count, Period period, KeyExtractorDelegate keyExtractor)
        {
            var policy = HttpManageRatePolicyBuilder.Build(keyExtractor, count, period)
                .WithName(policyName);
            _policies.Add(policy);

            return this;
        }

        public ManageRatesConfigurationBuilder AddNamedPolicy(string policyName, PolicyDelegate policyDelegate)
        {
            var policy = HttpManageRatePolicyBuilder.Build(policyDelegate)
                .WithName(policyName);

            _policies.Add(policy);

            return this;
        }

        public ManageRatesConfiguration Build()
        {
            return new ManageRatesConfiguration() { Policies = _policies.ToList() };
        }
    }
}
