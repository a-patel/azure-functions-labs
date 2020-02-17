#region Imports
using AzureFunctionsLabs.TimerTrigger.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
#endregion

namespace AzureFunctionsLabs.TimerTrigger
{
    public class TimerFunctions
    {
        #region Members

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

        #endregion

        #region Utilities

        #endregion
    }
}



#region @@Reference
/*

 
*/
#endregion
