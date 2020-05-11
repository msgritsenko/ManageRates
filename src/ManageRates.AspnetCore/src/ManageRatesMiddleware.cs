using ManageRates.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;

namespace ManageRates.AspnetCore
{
    public class ManageRatesMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly ManageRatesConfiguration _manageRatesOptions;
        private readonly ManageRatesService _manageRatesService;


        public ManageRatesMiddleware(
            RequestDelegate next,
            ManageRatesConfiguration configuration,
            ManageRatesService manageRatesService)
        {
            
        }
    }
}
