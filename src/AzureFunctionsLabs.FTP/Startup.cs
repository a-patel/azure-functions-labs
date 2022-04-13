#region Imports
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
#endregion

[assembly: FunctionsStartup(typeof(AzureFunctionsLabs.FTP.Startup))]
namespace AzureFunctionsLabs.FTP
{
    public class Startup : FunctionsStartup
    {
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            //builder.Services.AddSingleton<IFTPService, FTPService>();
        }
    }
}



#region @@Reference
/*
https://docs.microsoft.com/en-us/azure/azure-functions/functions-dotnet-dependency-injection
https://gunnarpeipman.com/azure-functions-dependency-injection/
 
*/
#endregion