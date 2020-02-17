using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventHubs;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace AzureFunctionsLabs.IoTHubTriggerDemo
{
    public static class Function1
    {
        private static HttpClient client = new HttpClient();

        [FunctionName("Function1")]
        public static void Run([IoTHubTrigger("messages/events", Connection = "IoTHubConnectionString")]EventData message, ILogger log)
        {
            log.LogInformation($"C# IoT Hub trigger function processed a message: {Encoding.UTF8.GetString(message.Body.Array)}");
        }
    }
}