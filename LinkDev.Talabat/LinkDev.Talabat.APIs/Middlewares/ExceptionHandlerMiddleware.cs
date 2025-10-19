using Azure;
using LinkDev.Talabat.APIs.Controllers.Errors;
using LinkDev.Talabat.Core.Application.Exceptions;
using System.Net;

namespace LinkDev.Talabat.APIs.Middlewares
{
    //Convension-Based
    public class ExceptionHandlerMiddleware
    {
        // RequestDelegate is a delegate that points to a method that takes (HttpContext)
        private readonly RequestDelegate _next; // points to the next middleware in the pipeline
        private readonly ILogger<ExceptionHandlerMiddleware> _logger; // for logging errors 
        private readonly IWebHostEnvironment _environment; // to determine if we're in development or production mode

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger, IWebHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        // called for every http request
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                // logic executed with request

                await _next(httpContext); // to proceed to the next middleware

                // logic executed with response

            }
            catch (Exception ex)
            {
                #region Logging : TODO
                if (_environment.IsDevelopment())
                {
                    _logger.LogError(ex, ex.Message);
                }
                else
                {
                    // Production mode
                    // Log exception details in db or file
                }
                await HanldeExceptionAsync(httpContext, ex); 
                #endregion
            }
        }

        private async Task HanldeExceptionAsync(HttpContext httpContext, Exception ex)
        {
            ApiResponse response;

            switch (ex)
            {
                case NotFoundException:
                    httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    httpContext.Response.ContentType = "application/json";

                    response = new ApiResponse((int)HttpStatusCode.NotFound, ex.Message);

                    await httpContext.Response.WriteAsync(response.ToString());

                    break;

                case ValidationException validationException:

                    httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    httpContext.Response.ContentType = "application/json";

                    response = new ApiValidationErrorResponse(ex.Message) { Errors = validationException.Errors};

                    await httpContext.Response.WriteAsync(response.ToString());

                    break;
                case BadRequestException:
                    httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    httpContext.Response.ContentType = "application/json";

                    response = new ApiResponse((int)HttpStatusCode.BadRequest, ex.Message);

                    await httpContext.Response.WriteAsync(response.ToString());

                    break;

                case UnAuthorizedException:
                    httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    httpContext.Response.ContentType = "application/json";

                    response = new ApiResponse((int)HttpStatusCode.Unauthorized, ex.Message);

                    await httpContext.Response.WriteAsync(response.ToString());

                    break;

                // to handle unexpected exceptions as stack overflow, db connection string, ..
                default:
                    response = _environment.IsDevelopment()
                        ? new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace?.ToString())
                        : new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);
 

                    // in the header of the response
                    httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    httpContext.Response.ContentType = "application/json";

                    // write response
                    await httpContext.Response.WriteAsync(response.ToString());
                    break;
            }
        }
    }
}