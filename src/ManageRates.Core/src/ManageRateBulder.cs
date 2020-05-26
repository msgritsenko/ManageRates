using ManageRates.Core.Abstractions;
using ManageRates.Core.Extensions;
using ManageRates.Core.Model;
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
        /// <param name="rateCount">Max rate in strict period.</param>
        /// <param name="ratePeriod">Rate striction period.</param>
        /// <returns></returns>
        public static IKeyedManageRatePolicy BuildKeyedDefaultPolicy(int rateCount, TimeSpan ratePeriod)
        {
            return new KeyedTimeRatePolicy(rateCount, ratePeriod);
        }

        /// <summary>
        /// Builds <see cref="IKeyedManageRatePolicy"/> with <see cref="KeyedTimeRatePolicy"/> as most common type.
        /// </summary>
        /// <param name="rateCount">Max rate in strict period.</param>
        /// <param name="ratePeriod">Predefined striction type period.</param>
        /// <returns></returns>
        public static IKeyedManageRatePolicy BuildKeyedDefaultPolicy(int rateCount, Period ratePeriod)
        {
            return BuildKeyedDefaultPolicy(rateCount, ratePeriod.ToTimeSpan());
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
        public static IManageRatePolicy BuildDefaultPolicy(Period ratePeriod, int rateCount)
        {
            return BuildDefaultPolicy(ratePeriod.ToTimeSpan(), rateCount);
        }
    }
}
