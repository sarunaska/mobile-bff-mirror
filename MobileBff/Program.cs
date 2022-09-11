using A3SClient.Extensions;
using AdapiClient.Extensions;
using MobileBff.Configurations;
using MobileBff.ExtensionMethods;
using MobileBff.Middlewares;
using MobileBff.Services;
using MobileBff.Services.ResponseValidation;
using MobileBff.Utilities;
using SebCsClient.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .UseCustomValidationErrorHandler();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.ToString());
    SebOpenAPIHelper.AddInfo(options,  builder.Configuration.GetSection("Godzilla:Url").Value);
    SebOpenAPIHelper.AddServer(options, builder.Configuration.GetSection("OpenApi:DevUrl").Value, "dev");
    SebOpenAPIHelper.AddServer(options, builder.Configuration.GetSection("OpenApi:StgUrl").Value, "stage");
});

builder.Services.AddScoped<IPingAdapiService, PingAdapiService>();
builder.Services.AddScoped<ICorporateAccountsService, CorporateAccountsService>();
builder.Services.AddScoped<IPrivateAccountsService, PrivateAccountsService>();
builder.Services.AddScoped<IYouthAccountsService, YouthAccountsService>();
builder.Services.AddScoped<IResponseValidator, ResponseValidator>();
builder.Services.AddScoped<IJwtParser, JwtParser>();

builder.Services.AddAdapiClient(x => new AdapiClientConfiguration(builder.Configuration.GetSection("Adapi")));
builder.Services.AddA3SClient(x => new A3SClientConfiguration(builder.Configuration.GetSection("A3S")));
builder.Services.AddSebCsClient(x => new SebCsClientConfiguration(builder.Configuration.GetSection("SebCs")));

builder.Services.AddHttpClient<IPingAdapiService, PingAdapiService>(c =>
 c.BaseAddress = new Uri(builder.Configuration.GetSection("Adapi")["Url"]));

builder.Services.AddApiVersioning(s =>
{
    s.AssumeDefaultVersionWhenUnspecified = true;
    s.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    s.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(s =>
{
    s.GroupNameFormat = "'v'VVV";
    s.SubstituteApiVersionInUrl = true;
});

var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseSwagger();
    app.UseSwaggerUI(x =>
    {
        x.SwaggerEndpoint("v1/swagger.yaml", "BFF For Mobile App in Accounts and Payments");
    });
}

app.UseCustomExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<LanguageMiddleware>();

app.Run();

/// <summary>
/// Enabling public visibility for the usage in Tests
/// </summary>
public partial class Program
{
}