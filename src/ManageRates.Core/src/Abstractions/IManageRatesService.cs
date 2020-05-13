using ManageRates.Core.Model;
using System.Threading.Tasks;

namespace ManageRates.Core.Abstractions
{
    /// <summary>
    /// Represents a rates management service, which can be used to restrict usage of resource.
    /// </summary>
    public interface IManageRatesService
    {
        /// <summary>
        /// Calculate whether resource is available.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ManageRatesResult> Process(ManageRatesRequest request);
    }
}