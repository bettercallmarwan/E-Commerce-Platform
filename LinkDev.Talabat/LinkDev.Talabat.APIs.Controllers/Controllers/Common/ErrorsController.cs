using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LinkDev.Talabat.APIs.Controllers.Controllers.Common
{
    [ApiController]
    [Route("Errors/{Code}")]
    [ApiExplorerSettings(IgnoreApi = true)] // to make this controller not documented in swagger
    public class ErrorsController : ControllerBase
    {
        public IActionResult error(int code)
        {
            if(code == (int)HttpStatusCode.NotFound)
            {
                ApiResponse response = new ApiResponse((int)HttpStatusCode.NotFound, $"The endpoint is not foundddddddd");
                return NotFound(response);
            }
            return StatusCode(code, new ApiResponse(code));
        }
    }
}


