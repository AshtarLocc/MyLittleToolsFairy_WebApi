using Microsoft.AspNetCore.Mvc;

namespace myLittleToolsFairy.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public class BaseController : ControllerBase
    {
        protected IActionResult HandleResponse<T>(T result, int successStatusCode = StatusCodes.Status200OK)
        {
            if (result == null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Resource not found",
                    Status = StatusCodes.Status404NotFound,
                    Detail = "The requested resource could not be found."
                });
            }
            else if (result is IEnumerable<object> collectionResult)
            {
                if (!collectionResult.Any())
                {
                    return NotFound(new ProblemDetails
                    {
                        Title = "Resource not found",
                        Status = StatusCodes.Status404NotFound,
                        Detail = "The requested resource could not be found."
                    });
                }
            }
            return StatusCode(successStatusCode, result);
        }

        protected IActionResult HandleError(string message, int errorCode = StatusCodes.Status500InternalServerError)
        {
            return StatusCode(errorCode, new ProblemDetails
            {
                Title = "Internal Server Error",
                Status = errorCode,
                Detail = message
            });
        }
    }
}