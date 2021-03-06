﻿using ManageRates.Core.Abstractions;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace ManageRates.Core.Policies
{
    public sealed class DelegateKeyedRatePolicy : IKeyedManageRatePolicy
    {
        private readonly Func<string, ITimeService, bool> _policy;

        public DelegateKeyedRatePolicy(Func<string, ITimeService, bool> policy)
        {
            _policy = policy;
        }

        public bool IsPermitted(string key, ITimeService timeService, IMemoryCache memoryCache) => _policy(key, timeService);
    }
}
