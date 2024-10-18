using Microsoft.AspNetCore.Mvc;
using myLittleToolsFairy.Commons.Model;

namespace myLittleToolsFairy.Commons.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ApiResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResult), StatusCodes.Status500InternalServerError)]
    public class CommonController : ControllerBase
    {
    }
}