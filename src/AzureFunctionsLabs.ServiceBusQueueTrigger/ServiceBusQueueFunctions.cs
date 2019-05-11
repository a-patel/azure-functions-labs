#region Imports
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging; 
#endregion

namespace AzureFunctionsLabs.ServiceBusQueueTrigger
{
    public static class ServiceBusQueueFunctions
    {
        [FunctionName("ServiceBusQueueTrigger")]
        public static void Run([ServiceBusTrigger("myqueue", Connection = "ServiceBusConnectionString")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
        }
    }
}



#region @@Reference
/*

 
*/
#endregion

