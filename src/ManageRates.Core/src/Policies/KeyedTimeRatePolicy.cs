using ManageRates.Core.Abstractions;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;

namespace ManageRates.Core.Policies
{
    /// <summary>
    /// Implementation of <see cref="IKeyedManageRatePolicy"/> with time period restriction.
    /// </summary>
    public sealed class KeyedTimeRatePolicy : IKeyedManageRatePolicy
    {
        private readonly TimeSpan _ratePeriod;
        private readonly int _rateCount;

        /// <summary>
        /// Creates a new <see cref="KeyedTimeRatePolicy"/> with specified <paramref name="rateCount"/> in <paramref name="ratePeriod"/> period.
        /// </summary>
        /// <param name="ratePeriod"></param>
        /// <param name="rateCount"></param>
        /// <param name="timeService"></param>
        public KeyedTimeRatePolicy(
            TimeSpan ratePeriod,
            int rateCount)
        {
            _ratePeriod = ratePeriod;
            _rateCount = rateCount;
        }

        /// <inheritdoc/>
        public bool IsPermitted(string key, ITimeService timeService, IMemoryCache memoryCache)
        {
            var timeQueue = memoryCache.GetOrCreate(key, cacheEntry =>
            {
                cacheEntry.SlidingExpiration = _ratePeriod;
                return new Queue<DateTime>(_rateCount);
            });

            lock (timeQueue)
            {
                var currentTime = timeService.GetUTC();

                while (timeQueue.Count > 0)
                {
                    var lstTiime = timeQueue.Peek();
                    if ((currentTime - lstTiime) > _ratePeriod)
                        timeQueue.Dequeue();
                    else
                        break;
                }

                if (timeQueue.Count < _rateCount)
                {
                    timeQueue.Enqueue(currentTime);
                    return true;
                }
            }

            return false;
        }
    }
}
