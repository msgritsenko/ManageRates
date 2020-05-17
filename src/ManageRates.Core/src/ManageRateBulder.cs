using ManageRates.Core.Abstractions;
using ManageRates.Core.Extensions;
using ManageRates.Core.Policies;
using System;

namespace ManageRates.Core
{
    /// <summary>
    /// Build helper for some complex implementations.
    /// </summary>
    public static class ManageRateBulder
    {
        /// <summary>
        /// Builds <see cref="IKeyedManageRatePolicy"/> with <see cref="KeyedTimeRatePolicy"/> as most common type.
        /// </summary>
        /// <param name="ratePeriod">Rate striction period.</param>
        /// <param name="rateCount">Max rate in strict period.</param>
        /// <returns></returns>
        public static IKeyedManageRatePolicy BuildKeyedDefaultPolicy(TimeSpan ratePeriod, int rateCount)
        {
            return new KeyedTimeRatePolicy(ratePeriod, rateCount);
        }

        /// <summary>
        /// Builds <see cref="IKeyedManageRatePolicy"/> with <see cref="KeyedTimeRatePolicy"/> as most common type.
        /// </summary>
        /// <param name="ratePeriod">Predefined striction type period.</param>
        /// <param name="rateCount">Max rate in strict period.</param>
        /// <returns></returns>
        public static IKeyedManageRatePolicy BuildKeyedDefaultPolicy(RatesStrictPeriod ratePeriod, int rateCount)
        {
            return BuildKeyedDefaultPolicy(ratePeriod.ToTimeSpan(), rateCount);
        }

        /// <summary>
        /// Builds <see cref="IManageRatePolicy"/> with <see cref="TimeRatePolicy"/> as most common type.
        /// </summary>
        /// <param name="ratePeriod">Rate striction period.</param>
        /// <param name="rateCount">Max rate in strict period.</param>
        /// <returns></returns>
        public static IManageRatePolicy BuildDefaultPolicy(TimeSpan ratePeriod, int rateCount)
        {
            return new TimeRatePolicy(ratePeriod, rateCount);
        }

        /// <summary>
        /// Builds <see cref="IManageRatePolicy"/> with <see cref="TimeRatePolicy"/> as most common type.
        /// </summary>
        /// <param name="ratePeriod">Predefined striction type period.</param>
        /// <param name="rateCount">Max rate in strict period.</param>
        /// <returns></returns>
        public static IManageRatePolicy BuildDefaultPolicy(RatesStrictPeriod ratePeriod, int rateCount)
        {
            return BuildDefaultPolicy(ratePeriod.ToTimeSpan(), rateCount);
        }
    }
}
