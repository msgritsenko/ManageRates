using ManageRates.Core.Model;

namespace ManageRates.Core.Abstractions
{
    /// <summary>
    /// Represents a rates management service, which can be used to restrict usage of resource.
    /// </summary>
    public interface IManageRatesService
    {
        /// <summary>
        /// Calculate whether resource is available by key.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ManageRatesResult Process(KeyedManageRatesRequest request);
        
        /// <summary>
        /// Calculate whether resource is available.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ManageRatesResult Process(ManageRatesRequest request);
    }
}