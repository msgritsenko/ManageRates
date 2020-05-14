using ManageRates.Core.Abstractions;
using ManageRates.Core.Model;

namespace ManageRates.Core
{
    /// <summary>
    /// Default implementation of <see cref="IKeyedManageRatePolicy"/>.
    /// </summary>
    public class ManageRatesService : IManageRatesService
    {
        /// <summary>
        /// Creates new instance <see cref="ManageRatesService"/>.
        /// </summary>
        public ManageRatesService()
        {
        }

        /// <inheritdoc/>
        public ManageRatesResult Process(KeyedManageRatesRequest request)
        {
            var permitted = request.Policy.IsPermitted(request.Key);

            var result = new ManageRatesResult(permitted);
            return result;
        }

        /// <inheritdoc/>
        public ManageRatesResult Process(ManageRatesRequest request)
        {
            var permitted = request.Policy.IsPermitted();

            var result = new ManageRatesResult(permitted);
            return result;
        }
    }
}