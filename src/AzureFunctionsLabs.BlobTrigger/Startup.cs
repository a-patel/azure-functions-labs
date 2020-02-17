#region Imports
using AzureFunctionsLabs.BlobTrigger.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
#endregion

[assembly: FunctionsStartup(typeof(AzureFunctionsLabs.BlobTrigger.Startup))]
namespace AzureFunctionsLabs.BlobTrigger
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddScoped<IMediaService, MediaService>();
        }
    }
}



#region @@Reference
/*
https://docs.microsoft.com/en-us/azure/azure-functions/functions-dotnet-dependency-injection
https://gunnarpeipman.com/azure-functions-dependency-injection/
 
*/
#endregion