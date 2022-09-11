using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MobileBff.Utilities
{
    public static class SebOpenAPIHelper
    {
        public static void AddInfo(SwaggerGenOptions options, string url)
        {
            Uri.TryCreate(url, UriKind.Absolute, out Uri? teamsUri);

            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "MobileBff",
                Version = "1.0",
                Description = "BFF For Mobile App in Accounts and Payments",
                Contact = new OpenApiContact
                {
                    Name = "Godzilla Team",
                    Url = teamsUri,
                    Email = "payments.team.godzilla@seb.se"
                },
                Extensions = new Dictionary<string, IOpenApiExtension>
                {
                    {
                        "x-seb", new OpenApiObject
                        {
                            { "domain", new OpenApiString("DOM15") },
                            { "tribe", new OpenApiString("TRI55") },
                            { "art", new OpenApiString("ART86") },
                            { "team", new OpenApiString("TEAM447") },
                        }
                    },
                    {
                        "x-seb-api", new OpenApiObject
                        {
                            { "id", new OpenApiString("84d513d3-3a76-4dd2-a78d-66eb108b11d7") },
                        }
                    },
                    {
                        "x-seb-portal", new OpenApiObject
                        {
                            { "category", new OpenApiString("payments") },
                            { "state", new OpenApiString("active") },
                            { "summary", new OpenApiString("Provides BFF for SEB Mobile App") },
                        }
                    }
                }
            });
        }

        public static void AddServer(SwaggerGenOptions options, string url, string enviroment = "")
        {
            options.AddServer(new OpenApiServer()
            {
                Url = url,
                Extensions = !string.IsNullOrEmpty(enviroment) ? new Dictionary<string, IOpenApiExtension>
                {
                        { "x-environment", new OpenApiString(enviroment) }
                }
                : null
            });
        }
    }
}