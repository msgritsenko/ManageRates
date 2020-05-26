using ManageRates.Core.Model;
using System;

namespace ManageRates.Core.Extensions
{
    public static class RateStrictPeriodExtensions
    {
        /// <summary>
        /// Converts <see cref="Period"/> into <see cref="TimeSpan"/>.
        /// </summary>
        /// <param name="ratePeriod"></param>
        /// <returns></returns>
        public static TimeSpan ToTimeSpan(this Period ratePeriod) => ratePeriod switch
        {
            Period.None => TimeSpan.FromSeconds(0),
            Period.Second => TimeSpan.FromSeconds(1),
            Period.Minute => TimeSpan.FromMinutes(1),
            Period.Hour => TimeSpan.FromHours(1),

            _ => throw new NotImplementedException()
        };
    }
}
