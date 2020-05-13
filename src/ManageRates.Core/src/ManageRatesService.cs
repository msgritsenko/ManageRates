using ManageRates.Core.Abstractions;
using ManageRates.Core.Model;
using System.Threading.Tasks;

namespace ManageRates.Core
{
    /// <summary>
    /// Default implementation of <see cref="IManageRatePolicy"/>.
    /// </summary>
    public class ManageRatesService : IManageRatesService
    {
        public ManageRatesService()
        {
        }

        
        public async Task<ManageRatesResult> Process(ManageRatesRequest request)
        {
            var permitted = await request.Policy.IsPermitted(request.Key);

            var result = new ManageRatesResult(permitted);
            return result;
        }
    }
}
