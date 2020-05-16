using ManageRates.Core.Abstractions;
using ManageRates.Core.Model;
using Microsoft.AspNetCore.Http;

namespace ManageRates.AspnetCore.Abstractions
{
    /// <summary>
    /// The main interface for all rate management aspnet components.
    /// </summary>
    public interface IHttpContextRatePolicy
    {
        /// <summary>
        /// Decides by <paramref name="context"/> is the resource permitted.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public ManageRatesResult IsPermitted(HttpContext context, ITimeService timeService);

        /// <summary>
        /// Decides whether this policy acceptable to <paramref name="context"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool Accept(HttpContext context);
    }
}
