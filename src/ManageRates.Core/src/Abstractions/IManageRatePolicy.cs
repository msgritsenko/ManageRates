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
        /// <returns>Resource's availability.</returns>
        bool IsPermitted(ITimeService timeService);
    }
}
