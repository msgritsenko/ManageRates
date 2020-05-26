using ManageRates.Core.Abstractions;
using ManageRates.Core.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace ManageRates.AspnetCore.Abstractions
{
    /// <summary>
    /// The main interface for all rate management aspnet components.
    /// </summary>
    public interface IHttpManageRatePolicy
    {
        /// <summary>
        /// Decides whether this policy acceptable to <paramref name="context"/>.
        /// </summary>
        /// <param name="context">Request httpcontext.</param>
        /// <returns></returns>
        public bool Accept(HttpContext context);

        /// <summary>
        /// Decides by <paramref name="context"/> is the resource permitted.
        /// </summary>
        public ManageRatesResult IsPermitted(HttpContext context, ITimeService timeService, IMemoryCache memoryCache);

        /// <summary>
        /// Name if it is the named policy.
        /// </summary>
        /// <remarks>Next versions will fix this incapsulation problem</remarks>
        public string Name { get; set; }

        /// <summary>
        /// Name of the policy that should be used instead of this one.
        /// </summary>
        /// /// <remarks>Next versions will fix this incapsulation problem</remarks>
        public string ReferenceName { get; }
    }
}
