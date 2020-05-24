using ManageRates.Core.Model;
using System;

namespace ManageRates.Core.Extensions
{
    public static class RateStrictPeriodExtensions
    {
        /// <summary>
        /// Converts <see cref="RatesStrictPeriod"/> into <see cref="TimeSpan"/>.
        /// </summary>
        /// <param name="ratePeriod"></param>
        /// <returns></returns>
        public static TimeSpan ToTimeSpan(this RatesStrictPeriod ratePeriod) => ratePeriod switch
        {
            RatesStrictPeriod.None => TimeSpan.FromSeconds(0),
            RatesStrictPeriod.Second => TimeSpan.FromSeconds(1),
            RatesStrictPeriod.Minute => TimeSpan.FromMinutes(1),
            RatesStrictPeriod.Hour => TimeSpan.FromHours(1),

            _ => throw new NotImplementedException()
        };
    }
}
