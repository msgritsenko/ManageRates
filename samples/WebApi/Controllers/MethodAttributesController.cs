using Microsoft.AspNetCore.Mvc;
using ManageRates.AspnetCore;
using ManageRates.Core.Model;

namespace WebApi.Controllers
{
    /// <summary>
    /// Sample of using attributes for each method.
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public class MethodAttributesController : ControllerBase
    {
        // <endpoint_attribute_sample>
        [HttpGet]
        [ManageRate(2, RatesStrictPeriod.Second, RatesStricType.Endpoint)]
        public string Endpoint()
        {
            return nameof(Endpoint);
        }
        // </endpoint_attribute_sample>

        [HttpGet]
        [ManageRate(2, RatesStrictPeriod.Second, RatesStricType.User)]
        public new string User()
        {
            return nameof(User);
        }

        [HttpGet]
        [ManageRate(2, RatesStrictPeriod.Second, RatesStricType.Ip)]
        public string Ip()
        {
            return nameof(Ip);
        }
    }
}
