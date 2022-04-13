#region Imports
using AzureFunctionsLabs.HTTPTrigger.Configuration;
using AzureFunctionsLabs.HTTPTrigger.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
#endregion

[assembly: FunctionsStartup(typeof(AzureFunctionsLabs.HTTPTrigger.Startup))]
namespace AzureFunctionsLabs.HTTPTrigger
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddOptions<CloudflareOptions>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.GetSection("CloudflareOptions").Bind(settings);
                });

            builder.Services.AddHttpClient();

            builder.Services.AddScoped<IWebhookService, WebhookService>();
        }
    }
}



#region @@Reference
/*
https://docs.microsoft.com/en-us/azure/azure-functions/functions-dotnet-dependency-injection
https://gunnarpeipman.com/azure-functions-dependency-injection/
 
*/
#endregion
