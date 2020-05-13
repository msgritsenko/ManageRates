using ManageRates.Core.Abstractions;

namespace ManageRates.Core.Model
{
    /// <summary>
    /// Represents the request for a resource availbility check.
    /// </summary>
    public sealed class ManageRatesRequest
    {
        public string Key { get; }
        public IManageRatePolicy Policy { get; }

        public ManageRatesRequest(string key, IManageRatePolicy policy)
        {
            Key = key;
            Policy = policy;
        }
    }
}
