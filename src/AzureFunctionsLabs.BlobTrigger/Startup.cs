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
