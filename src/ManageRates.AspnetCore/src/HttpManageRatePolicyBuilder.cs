using ManageRates.AspnetCore.Abstractions;
using ManageRates.Core.Abstractions;
using ManageRates.Core.Extensions;
using ManageRates.Core.Model;
using ManageRates.Core.Policies;
using Microsoft.AspNetCore.Http;
using System;
using System.Text.RegularExpressions;
using static ManageRates.AspnetCore.HttpManageRatePolicy;

namespace ManageRates.AspnetCore
{
    /// <summary>
    /// Predefined types of manage rate policy.
    /// </summary>
    public enum RatesStricType
    {
        None,
        Endpoint,
        User,
        Ip
    }

    public static class HttpManageRatePolicyBuilder
    {
        private delegate string KeyExtractorDelegate(HttpContext context);

        public static IHttpManageRatePolicy Build(int count, RatesStrictPeriod period, RatesStricType strictType)
        {
            PolicyDelegate policy = BuildDelegate(count, period, strictType);

            return Build(policy);
        }

        public static IHttpManageRatePolicy Build(string pattern, int count, RatesStrictPeriod period, RatesStricType strictType)
        {
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);

            return Build(regex, count, period, strictType);
        }
        public static IHttpManageRatePolicy Build(string pattern, PolicyDelegate policy)
        {
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);

            return Build(regex, policy);
        }

        public static IHttpManageRatePolicy Build(Regex regex, int count, RatesStrictPeriod period, RatesStricType strictType)
        {
            PolicyDelegate policy = BuildDelegate(count, period, strictType);
            AcceptDelegate accept = (context) => regex.IsMatch(EndpoinExtractor(context));

            return Build(policy, accept);
        }

        public static IHttpManageRatePolicy Build(Regex regex, PolicyDelegate policy)
        {
            AcceptDelegate accept = (context) => regex.IsMatch(EndpoinExtractor(context));

            return Build(policy, accept);
        }

        public static IHttpManageRatePolicy Build(PolicyDelegate policy, AcceptDelegate accept = null)
        {
            if (accept != null)
                return new HttpManageRatePolicy(policy, accept);
            return new HttpManageRatePolicy(policy);
        }

        private static PolicyDelegate BuildDelegate(int count, RatesStrictPeriod period, RatesStricType strictType)
        {
            PolicyDelegate policy;

            var keyedPolicy = new KeyedTimeRatePolicy(count, period.ToTimeSpan());
            switch (strictType)
            {
                case RatesStricType.None:
                    policy = KeyedPolicyBuilder(x => "", keyedPolicy);
                    break;
                case RatesStricType.Endpoint:
                    policy = KeyedPolicyBuilder(EndpoinExtractor, keyedPolicy);
                    break;
                case RatesStricType.User:
                    policy = KeyedPolicyBuilder(UserExtractor, keyedPolicy);
                    break;
                case RatesStricType.Ip:
                    policy = KeyedPolicyBuilder(IpExtractor, keyedPolicy);
                    break;

                default:
                    throw new NotImplementedException();
            }

            return policy;
        }

        private static PolicyDelegate KeyedPolicyBuilder(KeyExtractorDelegate keyExtractor, IKeyedManageRatePolicy _keyedPolicy)
        {
            return (context, timeService, memoryCache) =>
                    new Core.Model.ManageRatesResult(_keyedPolicy.IsPermitted($"{keyExtractor.GetHashCode()}_{keyExtractor(context)}", timeService, memoryCache));
        }

        private static string UserExtractor(HttpContext context)
        {
            return context.User.Identity.Name;
        }

        private static string IpExtractor(HttpContext context)
        {
            return context.Connection.RemoteIpAddress.ToString();
        }

        private static string EndpoinExtractor(HttpContext context)
        {
            return context.Request.Path;
        }
    }
}
