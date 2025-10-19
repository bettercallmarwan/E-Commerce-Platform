using LinkDev.Talabat.APIs.Controllers.Errors;
using LinkDev.Talabat.APIs.Extensions;
using LinkDev.Talabat.APIs.Middlewares;
using LinkDev.Talabat.APIs.Services;
using LinkDev.Talabat.Core.Application;
using LinkDev.Talabat.Core.Application.Abstraction;
using LinkDev.Talabat.Infrastructure.Persistence;
using LinkDev.Talabat.Inftrastructure;
using Microsoft.AspNetCore.Mvc;
namespace LinkDev.Talabat.APIs
{
    public class Program
    {

        public static async Task Main(string[] args)
        {

            var webApplicationBuilder = WebApplication.CreateBuilder(args);

            #region Configure services

            // Add services to the container. (DI)


            // my code
            webApplicationBuilder.Services
                .AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressModelStateInvalidFilter = false;
                    // InvalidModelStateResponseFactory property expects a function that:
                    // Takes an ActionContext as input
                    // Returns an IActionResult as output
                    options.InvalidModelStateResponseFactory = (actionContext) =>// ActionContext is a class that contains information about the current HTTP request and the action (controller method) being executed
                    {
                        var errors = actionContext.ModelState.Where(P => P.Value!.Errors.Count() > 0)
                                            .SelectMany(P => P.Value!.Errors.Select(E => E.ErrorMessage));


                        return new BadRequestObjectResult(new ApiValidationErrorResponse()
                        {
                            Errors = errors
                        });
                    };
                })
                .AddApplicationPart(typeof(LinkDev.Talabat.APIs.Controllers.AssemblyInformation).Assembly);




            // ahmed nasr code


            //webApplicationBuilder.Services.Configure<ApiBehaviorOptions>(options =>
            //{
            //    options.SuppressModelStateInvalidFilter = false;
            //    options.InvalidModelStateResponseFactory = (actionContext) =>
            //    {
            //        var errors = actionContext.ModelState.Where(p => p.Value!.Errors.Count() > 0)
            //                            .SelectMany(p => p.Value!.Errors)
            //                            .Select(E => E.ErrorMessage);

            //        return new BadRequestObjectResult(new ApiValidationErrorResponse()
            //        {
            //            Errors = errors
            //        });
            //    };
            //});
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            webApplicationBuilder.Services.AddEndpointsApiExplorer();
            webApplicationBuilder.Services.AddSwaggerGen();
            webApplicationBuilder.Services.AddHttpContextAccessor();
            webApplicationBuilder.Services.AddScoped(typeof(ILoggedInUserService), typeof(LoggedInUserService));

            webApplicationBuilder.Services.AddCors(corsOptions =>
            {
                corsOptions.AddPolicy("TalabatPolicy", policyBuilder =>
                {
                    policyBuilder.AllowAnyHeader().AllowAnyMethod().WithOrigins(webApplicationBuilder.Configuration["Urls:FrontBaseUrl"]!);
                });
            });
            webApplicationBuilder.Services.AddApplicationServices();
            webApplicationBuilder.Services.AddPersistenceServices(webApplicationBuilder.Configuration);
            webApplicationBuilder.Services.AddInfrastrcutureServices(webApplicationBuilder.Configuration);

            webApplicationBuilder.Services.AddIdentityServices(webApplicationBuilder.Configuration);


            #endregion

            var webApplication = webApplicationBuilder.Build();


            #region Apply Pending Migrations and Data Seeding
            // Asking runtime env for an object of StoreContext service explicitly
            await webApplication.InitializeDbAsync();
            #endregion

            #region Configure kestrel middlewares

            // Configure the HTTP request pipeline.

            // Register Swagger middleware before exception handling
            if (webApplication.Environment.IsDevelopment())
            {
                webApplication.UseSwagger();
                webApplication.UseSwaggerUI();
            }

            webApplication.UseMiddleware<ExceptionHandlerMiddleware>();

            webApplication.UseHttpsRedirection(); // direct ant http to be https

            webApplication.UseStatusCodePagesWithReExecute("/Errors/{0}"); // This middleware handles non-exceptional status codes (like 404, 401, 403) by re-executing the request pipeline to a new path instead of just returning a plain status code response
            //If a user hits a non-existing endpoint(/ api / invalid),
            //Instead of returning a plain 404,
            //ASP.NET will internally re-execute the pipeline and go to: /Errors/404

            webApplication.UseStaticFiles();

            webApplication.UseCors("TalabatPolicy");

            webApplication.UseAuthentication();
            webApplication.UseAuthorization();


            webApplication.MapControllers(); 
            #endregion

            webApplication.Run();
        }
    }
}
