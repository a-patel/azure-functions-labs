#region Imports
using AzureFunctionsLabs.TimerTrigger.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
#endregion

[assembly: FunctionsStartup(typeof(AzureFunctionsLabs.TimerTrigger.Startup))]
namespace AzureFunctionsLabs.TimerTrigger
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient();

            builder.Services.AddSingleton<IBackgroundJobService, BackgroundJobService>();
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