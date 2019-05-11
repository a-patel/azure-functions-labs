#region Imports
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging; 
#endregion

namespace AzureFunctionsLabs.TimerTrigger
{
    public static partial class TimerFunctions
    {
        #region Fields

        // {second=0} {minute=15} {hour=10} {day} {month} {day-of-week=(2=Tuesday)}
        private const string RemoveLogsSchedule = "0 15 10 * * 2";

        #endregion

        [FunctionName("RemoveLogs")]
        public static async Task RemoveLogs([TimerTrigger(RemoveLogsSchedule)]TimerInfo myTimer, ILogger log)
        {
            try
            {
                log.LogInformation($"Function executed at: {DateTime.Now}");

                var connectionString = Environment.GetEnvironmentVariable("SQLDbConnection");
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var query = "DELETE FROM [dbo].[Logs] WHERE [CreatedOn] > DATEADD(DAY, -2, GETUTCDATE())";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        var result = await cmd.ExecuteNonQueryAsync();
                        log.LogInformation($"Deleted GeoLocations: {result}");
                    }
                }

                log.LogInformation($"Function execution finished at: {DateTime.Now}");
            }
            catch (Exception ex)
            {
                log.LogError("Exception", ex);
                throw;
            }
        }
    }
}



#region @@Reference
/*

 
*/
#endregion

