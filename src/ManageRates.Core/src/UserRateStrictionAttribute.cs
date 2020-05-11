namespace ManageRates.Core
{
    /// <summary>
    /// Special User rate striction attribute.
    /// </summary>
    /// <remarks>Just only as a short version of <see cref="RateStrictionAttribute"/>.</remarks>
    public class UserRateStrictionAttribute : RateStrictionAttribute
    {
        public UserRateStrictionAttribute(
            int count = -1,
            RatesStrictPeriod period = RatesStrictPeriod.None)
            : base(RatesStrictsMode.User, count, period)
        {
        }
    }
}
