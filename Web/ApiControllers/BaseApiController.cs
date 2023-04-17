using Microsoft.AspNetCore.Mvc;
using Web.Filters;

namespace Web.ApiControllers
{
    /// <summary>
    /// The base API controller
    /// </summary>
    /// <seealso cref="ControllerBase" />
    [ApiKeyAuth]
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {

    }
}
