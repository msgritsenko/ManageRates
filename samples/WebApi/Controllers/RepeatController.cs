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

    /// <summary>
    /// Sample of rate limit for whole cotroller, 
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [IPRateStriction(4, RatesStrictPeriod.Second)]
    public class RepeatController : ControllerBase
    {
        private readonly ILogger<RepeatController> _logger;

        public RepeatController(ILogger<RepeatController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Index(string message)
        {
            return message;
        }
    }
}
