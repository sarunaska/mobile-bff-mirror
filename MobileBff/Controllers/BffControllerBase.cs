using Microsoft.AspNetCore.Mvc;
using MobileBff.Models;
using MobileBff.Services.ResponseValidation;

namespace MobileBff.Controllers
{
    public class BffControllerBase : ControllerBase
    {
        private readonly IResponseValidator responseValidator;

        public BffControllerBase(IResponseValidator responseValidator)
        {
            this.responseValidator = responseValidator;
        }

        public ObjectResult OkOrPartialContent<T>(T model)
            where T : IPartialResponseModel
        {
            var isValid = responseValidator.ValidateAndUpdate(model);
            var statusCode = isValid ? StatusCodes.Status200OK : StatusCodes.Status206PartialContent;

            return StatusCode(statusCode, model);
        }
    }
}
