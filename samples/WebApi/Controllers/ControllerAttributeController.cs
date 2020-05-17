using Microsoft.AspNetCore.Mvc;
using ManageRates.Core;

namespace WebApi.Controllers
{
    /// <summary>
    /// Sample of using attributes for whole controller.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [EndpointManageRate(2, RatesStrictPeriod.Second)]
    public class ControllerAttributeController : ControllerBase
    {

        [HttpGet]
        public string Index()
        {
            return "index";
        }
    }
}
