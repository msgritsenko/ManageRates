using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ManageRates.Core;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class YesNoController : ControllerBase
    {
        private readonly ILogger<YesNoController> _logger;

        public YesNoController(ILogger<YesNoController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [EndpointManageRate(2, RatesStrictPeriod.Second)]
        public string Yes()
        {
            _logger.LogInformation("called method{method}", nameof(Yes));
            return "yes";
        }


        [HttpGet]
        [UserManageRate(2, RatesStrictPeriod.Second)]
        public string No()
        {
            _logger.LogInformation("called method{method}", nameof(No));
            return "no";
        }

        [HttpGet]
        [IpManageRate(2, RatesStrictPeriod.Second)]
        public string Fail()
        {
            _logger.LogInformation("called method{method}", nameof(Fail));
            return $"fail";
        }
    }
}
