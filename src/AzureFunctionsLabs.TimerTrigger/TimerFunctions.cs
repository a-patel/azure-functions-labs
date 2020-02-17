#region Imports
using AzureFunctionsLabs.TimerTrigger.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
#endregion

namespace AzureFunctionsLabs.TimerTrigger
{
    public class TimerFunctions
    {
        #region Members

        private readonly IBackgroundJobService _backgroundJobService;
        private readonly IWebhookService _webhookService;

        // {second=0} {minute=15} {hour=10} {day} {month} {day-of-week=(2=Tuesday)}
        private const string REMOVELOGS_SCHEDULE = "0 15 10 * * 2";
        private const string WEBHOOK_SCHEDULE = "0 */5 * * * *";

        #endregion

        #region Ctor

        public TimerFunctions(IBackgroundJobService backgroundJobService, IWebhookService webhookService)
        {
            _backgroundJobService = backgroundJobService;
            _webhookService = webhookService;
        }

        #endregion

        #region Functions

        [FunctionName("TimerTrigger")]
        public void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }


        [FunctionName("RemoveLogs")]
        public async Task RemoveLogs([TimerTrigger(REMOVELOGS_SCHEDULE)]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"Deleted logs executed at: {DateTime.Now}");

            await _backgroundJobService.RemoveLogs();

            log.LogInformation($"Deleted logs execution finished at: {DateTime.Now}");
        }


        [FunctionName("CallWebhook")]
        public async Task CallWebhook([TimerTrigger(WEBHOOK_SCHEDULE)]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"CallWebhook executed at: {DateTime.Now}");

            await _webhookService.CallWebhook();

            log.LogInformation($"CallWebhook execution finished at: {DateTime.Now}");
        }

        #endregion

        #region Utilities

        #endregion
    }
}



#region @@Reference
/*

 
*/
#endregion
