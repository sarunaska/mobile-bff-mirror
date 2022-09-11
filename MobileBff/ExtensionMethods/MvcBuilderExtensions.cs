using Microsoft.AspNetCore.Mvc;
using MobileBff.Models.Errors;

namespace MobileBff.ExtensionMethods
{
    public static class MvcBuilderExtensions
    {
        public static void UseCustomValidationErrorHandler(this IMvcBuilder builder)
        {
            builder.ConfigureApiBehaviorOptions(setupAction =>
            {
                setupAction.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState.Values
                        .SelectMany(modelStateEntry => modelStateEntry.Errors.Select(modelError => modelError.ErrorMessage))
                        .ToArray();

                    var errorModel = new ValidationErrorModel(errors);

                    return new BadRequestObjectResult(errorModel)
                    {
                        ContentTypes = { "application/json" }
                    };
                };
            });
        }
    }
}
