#region Imports
using AzureFunctionsLabs.HTTPTrigger.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
#endregion

[assembly: FunctionsStartup(typeof(AzureFunctionsLabs.HTTPTrigger.Startup))]
namespace AzureFunctionsLabs.HTTPTrigger
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient();

            builder.Services.AddScoped<IWebhookService, WebhookService>();
        }
    }
}
