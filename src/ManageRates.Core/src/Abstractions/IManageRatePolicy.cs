using System.Threading.Tasks;

namespace ManageRates.Core.Abstractions
{
    /// <summary>
    /// Represents an object which decides accessibility of a resource.
    /// </summary>
    public interface IManageRatePolicy
    {
        /// <summary>
        /// Calculates whether a resource permitted for using.
        /// </summary>
        /// <param name="key">Identificator of a resource.</param>
        /// <returns></returns>
        Task<bool> IsPermitted(string key);
    }
}
