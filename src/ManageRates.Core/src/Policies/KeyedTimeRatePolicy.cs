using ManageRates.Core.Abstractions;
using System;
using System.Collections.Concurrent;
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
        private readonly ITimeService _timeService;
        private readonly ConcurrentDictionary<string, Queue<DateTime>> _timeStorage;

        /// <summary>
        /// Creates a new <see cref="KeyedTimeRatePolicy"/> with specified <paramref name="rateCount"/> in <paramref name="ratePeriod"/> period.
        /// </summary>
        /// <param name="ratePeriod"></param>
        /// <param name="rateCount"></param>
        /// <param name="timeService"></param>
        public KeyedTimeRatePolicy(
            TimeSpan ratePeriod, 
            int rateCount,
            ITimeService timeService)
        {
            _ratePeriod = ratePeriod;
            _rateCount = rateCount;
            _timeService = timeService;
            _timeStorage = new ConcurrentDictionary<string, Queue<DateTime>>();
        }

        /// <inheritdoc/>
        public bool IsPermitted(string key)
        {
            var timeQueue = _timeStorage.GetOrAdd(key, key => new Queue<DateTime>(_rateCount));

            lock (timeQueue)
            {
                var currentTime = _timeService.GetUTC();

                if (timeQueue.Count >= 0)
                {
                    var lstTiime = timeQueue.Peek();
                    if ((currentTime - lstTiime) > _ratePeriod)
                        timeQueue.Dequeue();
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
