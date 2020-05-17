using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ManageRates.Core;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EndpointManageRate(2, RatesStrictPeriod.Second)]
    public class RepeatController : ControllerBase
    {
        private readonly ILogger<RepeatController> _logger;

        public RepeatController(ILogger<RepeatController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Index()
        {
            return "index";
        }
    }
}
