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

        // {second=0} {minute=15} {hour=10} {day} {month} {day-of-week=(2=Tuesday)}
        private const string RemoveLogsSchedule = "0 15 10 * * 2";

        #endregion

        #region Ctor

        public TimerFunctions(IBackgroundJobService backgroundJobService)
        {
            _backgroundJobService = backgroundJobService;
        }

        #endregion

        #region Functions

        [FunctionName("TimerTrigger")]
        public void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }

        [FunctionName("RemoveLogs")]
        public async Task RemoveLogs([TimerTrigger(RemoveLogsSchedule)]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"Deleted logs executed at: {DateTime.Now}");

            await _backgroundJobService.RemoveLogs();

            log.LogInformation($"Deleted logs execution finished at: {DateTime.Now}");
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
