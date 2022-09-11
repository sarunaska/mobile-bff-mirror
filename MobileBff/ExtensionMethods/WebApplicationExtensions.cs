using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using MobileBff.Models.Errors;

namespace MobileBff.ExtensionMethods
{
    public static class WebApplicationExtensions
    {
        public static void UseCustomExceptionHandler(this WebApplication app)
        {
            app.UseExceptionHandler(exceptionHandlerApp =>
             {
                 exceptionHandlerApp.Run(async httpContext =>
                 {
                     httpContext.Response.ContentType = "application/json";
                     httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

                     var addDevelopmentDetails = app.Environment.IsDevelopment() || app.Environment.IsStaging();

                     var exceptionHandlerFeature = httpContext.Features.Get<IExceptionHandlerFeature>()!;
                     var errorModel = new UnhandledErrorModel(exceptionHandlerFeature.Error, addDevelopmentDetails);

                     var result = JsonSerializer.Serialize(errorModel);
                     await httpContext.Response.WriteAsync(result);

                     Console.Error.WriteLine($"Unhandled error. ID: {errorModel.Id}. Type: {errorModel.Type}. Message: {errorModel.Message}. StackTrace: {exceptionHandlerFeature.Error.StackTrace}");
                 });
             });
        }
    }
}
