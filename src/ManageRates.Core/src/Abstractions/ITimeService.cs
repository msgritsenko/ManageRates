using System;

namespace ManageRates.Core.Abstractions
{
    /// <summary>
    /// Represents interface for getting current time in order to testability and collaboration with other systems.
    /// </summary>
    public interface ITimeService
    {
        /// <summary>
        /// Returns current time.
        /// </summary>
        /// <returns>Current DateTime in universal coordinated time format.</returns>
        DateTime GetUTC();
    }
}
