#region Imports
using AzureFunctionsLabs.AzureAppConfiguration.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
#endregion

[assembly: FunctionsStartup(typeof(AzureFunctionsLabs.AzureAppConfiguration.Startup))]
namespace AzureFunctionsLabs.AzureAppConfiguration
{
    public class Startup : FunctionsStartup
    {
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            string connectionString = Environment.GetEnvironmentVariable("AzureAppConfigurationConnectionString");
            builder.ConfigurationBuilder.AddAzureAppConfiguration(connectionString);
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<INotificationService, NotificationService>();
        }
    }
}



#region @@Reference
/*
https://docs.microsoft.com/en-us/azure/azure-functions/functions-dotnet-dependency-injection
https://gunnarpeipman.com/azure-functions-dependency-injection/
 
*/
#endregion