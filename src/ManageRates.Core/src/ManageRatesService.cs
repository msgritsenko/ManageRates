using Microsoft.Extensions.Caching.Memory;
using System;

namespace ManageRates.Core
{
    public class ManageRatesService
    {
        private readonly IMemoryCache _memoryCache;
        public ManageRatesService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }


    }
}
