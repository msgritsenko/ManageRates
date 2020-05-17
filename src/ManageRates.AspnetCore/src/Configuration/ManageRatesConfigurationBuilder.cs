using ManageRates.AspnetCore.Abstractions;
using ManageRates.AspnetCore.Policies;
using ManageRates.Core;
using ManageRates.Core.Abstractions;
using ManageRates.Core.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ManageRates.AspnetCore.Configuration
{
    public class ManageRatesConfigurationBuilder
    {
        private readonly List<IHttpContextRatePolicy> _policies;
        public ManageRatesConfigurationBuilder()
        {
            _policies = new List<IHttpContextRatePolicy>();
        }

        public ManageRatesConfigurationBuilder AddManageRates(string patter, int count, RatesStrictPeriod period)
        {
            var innerPolicy = new EndpointManageRatePolicy(count, period);
            _policies.Add(new RegexManageRatePolicyDecorator(patter, innerPolicy));

            return this;
        }

        public ManageRatesConfigurationBuilder AddManageRatesByIp(string patter, int count, RatesStrictPeriod period)
        {
            var innerPolicy = new IpManageRatePolicy(count, period);
            _policies.Add(new RegexManageRatePolicyDecorator(patter, innerPolicy));

            return this;
        }

        public ManageRatesConfigurationBuilder AddManageRatesByUser(string patter, int count, RatesStrictPeriod period)
        {
            var innerPolicy = new UserManageRatePolicy(count, period);
            _policies.Add(new RegexManageRatePolicyDecorator(patter, innerPolicy));

            return this;
        }

        public ManageRatesConfigurationBuilder AddManageRatesByDelegate(string patter, Func<HttpContext, ITimeService, ManageRatesResult> policy)
        {
            var innerPolicy = new DelegateManageRatePolicy(policy);
            _policies.Add(new RegexManageRatePolicyDecorator(patter, innerPolicy));

            return this;
        }

        public ManageRatesConfiguration Build()
        {
            return new ManageRatesConfiguration() { Policies = _policies.ToList() };
        }
    }
}
