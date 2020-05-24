namespace ManageRates.Core.Model
{
    /// <summary>
    /// Supported restrict periods.
    /// </summary>
    public enum RatesStrictPeriod
    {
        /// <summary>
        /// Empty strict period.
        /// </summary>
        /// <remarks>Shouldn't be used.</remarks>
        None,

        /// <summary>
        /// One second strict period.
        /// </summary>
        Second,

        /// <summary>
        /// One minute strict period.
        /// </summary>
        Minute,

        /// <summary>
        /// One hour strict period.
        /// </summary>
        Hour
    }
}
