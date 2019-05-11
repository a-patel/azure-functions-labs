#region Imports
using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging; 
#endregion

namespace AzureFunctionsLabs.TimerTrigger
{
    public static partial class TimerFunctions
    {
        [FunctionName("TimerTrigger")]
        public static void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}



#region @@Reference
/*

 
*/
#endregion

