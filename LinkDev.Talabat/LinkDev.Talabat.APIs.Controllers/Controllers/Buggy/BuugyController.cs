using LinkDev.Talabat.APIs.Controllers.Base;
using LinkDev.Talabat.APIs.Controllers.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LinkDev.Talabat.APIs.Controllers.Controllers.Buggy
    {
        public class BuggyController : BaseApiController
        {
            [HttpGet("notfound")] // GET: /api/buggy/notfound
            public IActionResult GetNotFoundResult() // resources not found
            {
                 return NotFound(new ApiResponse(404));
            }


            [HttpGet("servererror")] // GET: /api/buggy/servererror
            public IActionResult GetServerError()
            {
                throw new Exception();
            }


           [HttpGet("badrequest")] // GET: /api/buggy/badrequest
           public IActionResult GetBadRequest() // client side error
           {
               return BadRequest(new ApiResponse(400)); //400
           }

           [HttpGet("badrequest/{id}/{name}")] // GET: /api/buggy/badrequest/five
           public IActionResult GetValidationError(int id, string name) // 400
           {

                return Ok();
            }


           [HttpGet("unauthorized")] // GET: /api/buggy/unauthorized
           public IActionResult GetUnauthorizedError()
           {
                return Unauthorized();
           }


            [HttpGet("forbidden")] // GET: /api/buggy/forbidden
            public IActionResult GetForbiddenError()
            {
                return Forbid(); //401
            }



            [Authorize]
            [HttpGet("authorized")]
            public IActionResult GetAuthorizedRequest()
            {
                return Ok();
            }

        }
    }
