using ManageRates.Core.Abstractions;

namespace ManageRates.Core.Model
{
    /// <summary>
    /// Represents the request for a resource availbility check.
    /// </summary>
    public sealed class KeyedManageRatesRequest
    {
        public string Key { get; }
        public IKeyedManageRatePolicy Policy { get; }

        public KeyedManageRatesRequest(string key, IKeyedManageRatePolicy policy)
        {
            Key = key;
            Policy = policy;
        }
    }
}
