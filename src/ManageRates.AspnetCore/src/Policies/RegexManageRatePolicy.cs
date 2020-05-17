using ManageRates.AspnetCore.Abstractions;
using ManageRates.Core.Abstractions;
using ManageRates.Core.Model;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;

namespace ManageRates.AspnetCore.Policies
{
    public class RegexManageRatePolicyDecorator : IHttpContextRatePolicy
    {
        private readonly Regex _regex;
        private readonly IHttpContextRatePolicy _innerPolicy;

        public RegexManageRatePolicyDecorator(string pattern, IHttpContextRatePolicy innerPolicy)
            : this(pattern, RegexOptions.IgnoreCase, innerPolicy)
        {
        }

        public RegexManageRatePolicyDecorator(string pattern, RegexOptions options, IHttpContextRatePolicy innerPolicy)
        {
            _regex = new Regex(pattern, options);
            _innerPolicy = innerPolicy;
        }

        public bool Accept(HttpContext context)
        {
            return _regex.IsMatch(context.Request.Path);
        }

        public ManageRatesResult IsPermitted(HttpContext context, ITimeService timeService)
        {
            return _innerPolicy.IsPermitted(context, timeService);
        }
    }
}
