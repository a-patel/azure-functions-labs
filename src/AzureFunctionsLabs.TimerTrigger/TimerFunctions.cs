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

        // {second=0} {minute=15} {hour=10} {day} {month} {day-of-week=(2=Tuesday)}
        private const string RemoveLogsSchedule = "0 15 10 * * 2";

        private readonly IBackgroundJobService _backgroundJobService;

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
            await _backgroundJobService.RemoveLogs();
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
