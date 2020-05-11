using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ManageRates.AspnetCore;
using ManageRates.Core;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class YesNoController : ControllerBase
    {
        private readonly ILogger<YesNoController> _logger;

        public YesNoController(ILogger<YesNoController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [RateStriction( RatesStrictsMode.User, 4, RatesStrictPeriod.Second)]
        public string Yes()
        {
            return "yes";
        }


        [HttpGet]
        [UserRateStriction(4, RatesStrictPeriod.Second)]
        public string No()
        {
            return "no";
        }

        [HttpGet]
        [IPRateStriction(4, RatesStrictPeriod.Second)]
        public string Fail(int failCode)
        {
            return $"fail {failCode}";
        }
    }
}
