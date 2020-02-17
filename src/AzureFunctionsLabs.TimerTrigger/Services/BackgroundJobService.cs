#region Imports
using Microsoft.Extensions.Logging;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

#endregion

namespace AzureFunctionsLabs.TimerTrigger.Services
{
    public class BackgroundJobService : IBackgroundJobService
    {
        #region Members

        private readonly ILogger _logger;
        private readonly string _connectionString;

        #endregion

        #region Ctor

        public BackgroundJobService(ILogger logger)
        {
            _logger = logger;
            _connectionString = Environment.GetEnvironmentVariable("DbConnectionString");
        }

        #endregion

        public async Task RemoveLogs()
        {
            try
            {
                await using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var query = "DELETE FROM [dbo].[Logs] WHERE [CreatedOn] > DATEADD(DAY, -2, GETUTCDATE())";

                    await using SqlCommand cmd = new SqlCommand(query, connection);
                    var result = await cmd.ExecuteNonQueryAsync();

                    _logger.LogInformation($"Total deleted logs: {result}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("RemoveLogs Exception", ex);
                throw;
            }
        }
    }
}
