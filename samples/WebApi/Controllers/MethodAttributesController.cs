using Microsoft.AspNetCore.Mvc;
using ManageRates.Core;

namespace WebApi.Controllers
{
    /// <summary>
    /// Sample of using attributes for each method.
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public class MethodAttributesController : ControllerBase
    {
        [HttpGet]
        [EndpointManageRate(2, RatesStrictPeriod.Second)]
        public string Endpoint()
        {
            return nameof(Endpoint);
        }


        [HttpGet]
        [UserManageRate(2, RatesStrictPeriod.Second)]
        public new string User()
        {
            return nameof(User);
        }

        [HttpGet]
        [IpManageRate(2, RatesStrictPeriod.Second)]
        public string Ip()
        {
            return nameof(Ip);
        }
    }
}
