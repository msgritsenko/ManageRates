namespace ManageRates.Core
{
    /// <summary>
    /// Special IP rate striction attribute.
    /// </summary>
    /// <remarks>Just only as a short version of <see cref="RateStrictionAttribute"/>.</remarks>
    public class IPRateStrictionAttribute : RateStrictionAttribute
    {
        public IPRateStrictionAttribute(
            int count = -1,
            RatesStrictPeriod period = RatesStrictPeriod.None)
            : base(RatesStrictsMode.IP, count, period)
        {
        }
    }
}
