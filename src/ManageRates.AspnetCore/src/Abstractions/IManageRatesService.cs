using ManageRates.AspnetCore.Configuration;
using ManageRates.Core.Model;
using Microsoft.AspNetCore.Http;

namespace ManageRates.AspnetCore.Abstractions
{
    /// <summary>
    /// Object wich manipulates context to privide limit of rate.
    /// </summary>
    public interface IManageRatesService
    {
        /// <summary>
        /// Decides whether resource is availabable.
        /// </summary>
        /// <param name="context">Http context must contain all necessary information.</param>
        /// <returns></returns>
        ManageRatesResult Process(HttpContext context, ManageRatesConfiguration policies = null);
    }
}