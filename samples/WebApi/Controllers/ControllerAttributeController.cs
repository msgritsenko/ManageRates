using Microsoft.AspNetCore.Mvc;
using ManageRates.AspnetCore;
using ManageRates.Core.Model;
using ManageRates.AspnetCore.Infrastructure;

namespace WebApi.Controllers
{
    /// <summary>
    /// Sample of using attributes for whole controller.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [ManageRate(2, RatesStrictPeriod.Second, RatesStricType.Endpoint)]
    public class ControllerAttributeController : ControllerBase
    {

        [HttpGet]
        public string Index()
        {
            return "index";
        }
    }
}
