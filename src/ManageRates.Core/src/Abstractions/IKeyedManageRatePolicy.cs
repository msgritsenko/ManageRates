namespace ManageRates.Core.Abstractions
{
    /// <summary>
    /// Represents an object which decides accessibility of a resource by key.
    /// </summary>
    public interface IKeyedManageRatePolicy
    {
        /// <summary>
        /// Calculates whether a resource permitted for using.
        /// </summary>
        /// <param name="key">Identificator of a resource.</param>
        /// <returns>Resource's availability.</returns>
        bool IsPermitted(string key);
    }
}
