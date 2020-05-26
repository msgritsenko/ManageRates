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
    public delegate string KeyExtractorDelegate(HttpContext context);

    public static class HttpManageRatePolicyBuilder
    {

        public static IHttpManageRatePolicy WithName(this IHttpManageRatePolicy policy, string name)
        {
            policy.Name = name;
            return policy;
        }

        public static IHttpManageRatePolicy Build(string name)
        {
            var policy = new HttpManageRatePolicy(null);
            policy.ReferenceName = name;

            return policy;
        }

        public static IHttpManageRatePolicy Build(int count, Period period, KeyType keyType)
        {
            PolicyDelegate policy = BuildKeyExtractorDelegate(count, period, keyType);

            return Build(policy);
        }

        public static IHttpManageRatePolicy Build(string pattern, int count, Period period, KeyType keyType)
        {
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);

            return Build(regex, count, period, keyType);
        }

        public static IHttpManageRatePolicy Build(string pattern, PolicyDelegate policy)
        {
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);

            return Build(regex, policy);
        }

        public static IHttpManageRatePolicy Build(Regex regex, int count, Period period, KeyType keyType)
        {
            PolicyDelegate policy = BuildKeyExtractorDelegate(count, period, keyType);
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

        public static IHttpManageRatePolicy Build(KeyExtractorDelegate keyExtractor, int count, Period period)
        {
            var keyedPolicy = new KeyedTimeRatePolicy(count, period.ToTimeSpan());

            PolicyDelegate policy = KeyedPolicyBuilder(keyExtractor, keyedPolicy);

            return new HttpManageRatePolicy(policy);
        }
        public static IHttpManageRatePolicy Build(string pattern, KeyExtractorDelegate keyExtractor, int count, Period period)
        {
            var keyedPolicy = new KeyedTimeRatePolicy(count, period.ToTimeSpan());

            PolicyDelegate policy = KeyedPolicyBuilder(keyExtractor, keyedPolicy);

            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            AcceptDelegate accept = (context) => regex.IsMatch(EndpoinExtractor(context));

            return new HttpManageRatePolicy(policy, accept);
        }

        private static PolicyDelegate BuildKeyExtractorDelegate(int count, Period period, KeyType keyType)
        {
            PolicyDelegate policy;

            var keyedPolicy = new KeyedTimeRatePolicy(count, period.ToTimeSpan());
            switch (keyType)
            {
                case KeyType.None:
                    policy = KeyedPolicyBuilder(x => "", keyedPolicy);
                    break;
                case KeyType.RequestPath:
                    policy = KeyedPolicyBuilder(EndpoinExtractor, keyedPolicy);
                    break;
                case KeyType.User:
                    policy = KeyedPolicyBuilder(UserExtractor, keyedPolicy);
                    break;
                case KeyType.Ip:
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
