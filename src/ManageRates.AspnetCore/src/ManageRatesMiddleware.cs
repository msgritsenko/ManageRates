using ManageRates.Core;
using Microsoft.AspNetCore.Http;

namespace ManageRates.AspnetCore
{
    public class ManageRatesMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ManageRatesConfiguration _configuration;
        private readonly ManageRatesService _manageRatesService;


        public ManageRatesMiddleware(
            RequestDelegate next,
            ManageRatesConfiguration configuration,
            ManageRatesService manageRatesService)
        {
            _next = next;
            _configuration = configuration;
            _manageRatesService = manageRatesService;
        }
    }
}
