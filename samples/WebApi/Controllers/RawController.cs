using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RawController : ControllerBase
    {
        #region User restrictions testing
        [HttpGet()]
        public string UserFunction()
        {
            return nameof(UserFunction);
        }

        [HttpGet()]
        public string FunctionUser()
        {
            return nameof(FunctionUser);
        }
        #endregion

        #region Ip restrictions testing
        [HttpGet()]
        public string IpFunction()
        {
            return nameof(IpFunction);
        }

        [HttpGet()]
        public string FunctionIp()
        {
            return nameof(FunctionIp);
        }
        #endregion

        #region Endpoint restrictions testing
        [HttpGet()]
        public string EndpointFunction()
        {
            return nameof(EndpointFunction);
        }

        [HttpGet()]
        public string FunctionEndpoint()
        {
            return nameof(FunctionEndpoint);
        }
        #endregion
    }
}