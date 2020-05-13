﻿namespace ManageRates.Core.Model
{
    /// <summary>
    /// Represents the result of a resource availbility check.
    /// </summary>
    public sealed class ManageRatesResult
    {
        /// <summary>
        /// Access to a resource permitted.
        /// </summary>
        public bool Permitted { get; }

        public ManageRatesResult(bool permitted)
        {
            Permitted = permitted;
        }
    }
}
