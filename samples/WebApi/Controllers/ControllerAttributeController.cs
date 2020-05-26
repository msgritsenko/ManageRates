using Microsoft.AspNetCore.Mvc;
using ManageRates.AspnetCore;
using ManageRates.Core.Model;
using ManageRates.AspnetCore.Infrastructure;

namespace WebApi.Controllers
{
    /// <summary>
    /// Sample of using attributes for whole controller.
    /// </summary>
    /// <remarks>
    /// Please, note the type of key <see cref="KeyType.RequestPath"/> this means
    /// that each action has own restriction. In this case we have 3 actions and count = 2, 
    /// so 2 * 3 = 6 maximum queries total within <see cref="Period.Second"/>.
    /// </remarks>
    [ApiController]
    [Route("[controller]/[action]")]
    [ManageRate(2, Period.Second, KeyType.RequestPath)]
    public class ControllerAttributeController : ControllerBase
    {

        [HttpGet]
        public string Index1()
        {
            return "index1";
        }

        [HttpGet]
        public string Index2()
        {
            return "index2";
        }

        [HttpGet]
        public string Index3()
        {
            return "index3";
        }
    }
}
