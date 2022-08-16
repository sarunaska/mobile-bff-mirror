using A3SClient.Extensions;
using AdapiClient.Extensions;
using MobileBff.Configuration;
using MobileBff.Middlewares;
using MobileBff.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.ToString());
});

builder.Services.AddScoped<IPingAdapiService, PingAdapiService>();
builder.Services.AddScoped<ICorporateAccountsService, CorporateAccountsService>();
builder.Services.AddScoped<IPrivateAccountsService, PrivateAccountsService>();

builder.Services.AddAdapiClient(x => new AdapiClientConfiguration(builder.Configuration.GetSection("Adapi")));
builder.Services.AddA3SClient(x => new A3SClientConfiguration(builder.Configuration.GetSection("A3S")));

builder.Services.AddHttpClient<IPingAdapiService, PingAdapiService>(c =>
 c.BaseAddress = new Uri(builder.Configuration.GetSection("Adapi")["Url"]));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

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