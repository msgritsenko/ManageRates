using ManageRates.AspnetCore.Abstractions;
using ManageRates.Core.Model;
using System.Collections.Generic;
using System.Linq;
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

        public ManageRatesConfigurationBuilder AddManageRates(string pattern, int count, RatesStrictPeriod period)
        {
            var policy = HttpManageRatePolicyBuilder.Build(pattern, count, period, RatesStricType.None);
            _policies.Add(policy);

            return this;
        }

        public ManageRatesConfigurationBuilder AddManageRatesByIp(string pattern, int count, RatesStrictPeriod period)
        {
            var policy = HttpManageRatePolicyBuilder.Build(pattern, count, period, RatesStricType.Ip);
            _policies.Add(policy);

            return this;
        }

        public ManageRatesConfigurationBuilder AddManageRatesByUser(string pattern, int count, RatesStrictPeriod period)
        {
            var policy = HttpManageRatePolicyBuilder.Build(pattern, count, period, RatesStricType.User);
            _policies.Add(policy);

            return this;
        }

        public ManageRatesConfigurationBuilder AddManageRatesByDelegate(string pattern, PolicyDelegate policyDelegate)
        {
            var policy = HttpManageRatePolicyBuilder.Build(pattern, policyDelegate);

            _policies.Add(policy);

            return this;
        }

        public ManageRatesConfiguration Build()
        {
            return new ManageRatesConfiguration() { Policies = _policies.ToList() };
        }
    }
}
