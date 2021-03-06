﻿using ManageRates.Core.Abstractions;

namespace ManageRates.Core.Model
{
    /// <summary>
    /// Represents the request for a resource availbility check.
    /// </summary>
    public sealed class ManageRatesRequest
    {
        public IManageRatePolicy Policy { get; }

        public ManageRatesRequest(IManageRatePolicy policy)
        {
            Policy = policy;
        }
    }
}
