using ManageRates.Core.Abstractions;
using System;

namespace ManageRates.Core.Policies
{
    public sealed class DelegateRatePolicy : IManageRatePolicy
    {
        private readonly Func<ITimeService, bool> _policy;

        public DelegateRatePolicy(Func<ITimeService, bool> policy)
        {
            _policy = policy;
        }

        public bool IsPermitted(ITimeService timeService) => _policy(timeService);
    }
}
