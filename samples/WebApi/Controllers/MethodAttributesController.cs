using Microsoft.AspNetCore.Mvc;
using ManageRates.AspnetCore;
using ManageRates.Core.Model;
using ManageRates.AspnetCore.Infrastructure;

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
        [ManageRate(2, Period.Second, KeyType.RequestPath)]
        public string Endpoint()
        {
            return nameof(Endpoint);
        }
        // </endpoint_attribute_sample>

        [HttpGet]
        [ManageRate(2, Period.Second, KeyType.User)]
        public new string User()
        {
            return nameof(User);
        }

        [HttpGet]
        [ManageRate(2, Period.Second, KeyType.Ip)]
        public string Ip()
        {
            return nameof(Ip);
        }

        [HttpGet]
        [ManageRate("NoMore10PerSecond")]
        public string NamedPolicy()
        {
            return nameof(NamedPolicy);
        }

    }
}
