#region Imports
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging; 
#endregion

namespace AzureFunctionsLabs.ServiceBusTopicTrigger
{
    public static class ServiceBusTopicFunctions
    {
        [FunctionName("ServiceBusTopicTrigger")]
        public static void Run([ServiceBusTrigger("mytopic", "mysubscription", Connection = "ServiceBusConnectionString")]string mySbMsg, ILogger log)
        {
            log.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
        }
    }
}



#region @@Reference
/*

 
*/
#endregion

