namespace MobileBff.Services.ResponseValidation
{
    public interface IResponseValidator
    {
        bool ValidateAndUpdate<T>(T model);
    }
}