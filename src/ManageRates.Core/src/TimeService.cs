using ManageRates.Core.Abstractions;
using System;

namespace ManageRates.Core
{
    /// <summary>
    /// The simpliest implementation of <see cref="ITimeService"/>.
    /// </summary>
    public class TimeService : ITimeService
    {
        /// <inheritdoc/>
        public DateTime GetUTC()
        {
            return DateTime.UtcNow;
        }
    }
}
