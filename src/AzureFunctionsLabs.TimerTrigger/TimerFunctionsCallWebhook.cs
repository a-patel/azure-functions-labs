#region Imports
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;
#endregion

namespace AzureFunctionsLabs.TimerTrigger
{
    public static partial class TimerFunctions
    {
        #region Fields

        // {second=0} {minute=15} {hour=10} {day} {month} {day-of-week=(2=Tuesday)}
        private const string TimerSchedule = "0 15 10 * * 2";
        private static HttpClient _client = new HttpClient();

        #endregion

        [FunctionName("CallWebhook")]
        public static async Task CallWebhook([TimerTrigger(TimerSchedule)]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"Tiggering CallWebhook at: {DateTime.Now}");

            var webHookUrl = Environment.GetEnvironmentVariable("WebHookUrl");

            await _client.PostAsJsonAsync(webHookUrl, "{}");

            log.LogInformation($"CallWebhook triggered successfully at: {DateTime.Now}");
        }
    }
}



#region @@Reference
/*
https://andrewlock.net/creating-my-first-azure-functions-v2-app-a-webhook-and-a-timer/
 
*/
#endregion

