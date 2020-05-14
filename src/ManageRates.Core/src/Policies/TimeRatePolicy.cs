using ManageRates.Core.Abstractions;
using System;
using System.Collections.Generic;

namespace ManageRates.Core.Policies
{
    /// <summary>
    /// Implementation of <see cref="IManageRatePolicy"/> with time period restriction.
    /// </summary>
    public sealed class TimeRatePolicy : IManageRatePolicy
    {
        private readonly TimeSpan _ratePeriod;
        private readonly int _rateCount;
        private readonly ITimeService _timeService;
        private readonly Queue<DateTime> _timeStorage;

        /// <summary>
        /// Creates a new <see cref="KeyedTimeRatePolicy"/> with specified <paramref name="rateCount"/> in <paramref name="ratePeriod"/> period.
        /// </summary>
        /// <param name="ratePeriod"></param>
        /// <param name="rateCount"></param>
        /// <param name="timeService"></param>
        public TimeRatePolicy(
            TimeSpan ratePeriod, 
            int rateCount,
            ITimeService timeService)
        {
            _ratePeriod = ratePeriod;
            _rateCount = rateCount;
            _timeService = timeService;
            _timeStorage = new Queue<DateTime>();
        }

        /// <inheritdoc/>
        public bool IsPermitted()
        {
            var timeQueue = _timeStorage;
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
