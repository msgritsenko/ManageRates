namespace ManageRates.Core
{
    /// <summary>
    /// Mode of processing restrictions.
    /// </summary>
    public enum RatesStrictsMode
    {
        /// <summary>
        /// Not to do any restrictions.
        /// </summary>
        None,

        /// <summary>
        /// Each user has own limits.
        /// </summary>
        User,

        /// <summary>
        /// Each IP has own limits.
        /// </summary>
        IP,

        /// <summary>
        /// Use limits regardless of user or IP.
        /// </summary>
        Endpoint
    }
}
