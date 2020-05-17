using ManageRates.AspnetCore.Abstractions;
using System.Collections.Generic;

namespace ManageRates.AspnetCore.Configuration
{
    public class ManageRatesConfiguration
    {
        public IReadOnlyList<IHttpContextRatePolicy> Policies { get; set; }
    }
}
