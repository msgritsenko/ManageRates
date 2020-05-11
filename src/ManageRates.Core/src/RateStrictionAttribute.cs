using System;

namespace ManageRates.Core
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class RateStrictionAttribute : Attribute
    {
        public RatesStrictsMode Mode { get; }
        
        public int Count { get; }

        public RatesStrictPeriod Period { get; }
        
        public RateStrictionAttribute(
            RatesStrictsMode mode = RatesStrictsMode.None, 
            int count = -1,
            RatesStrictPeriod period = RatesStrictPeriod.None)
        {
            Mode = mode;
            Count = count;
            Period = period;
        }
    }
}
