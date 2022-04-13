#region Imports
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

#endregion

namespace AzureFunctionsLabs.AzureAppConfiguration.Services
{
    public class NotificationService : INotificationService
    {
        #region Members

        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        #endregion

        #region Ctor

        public NotificationService(IConfiguration configuration, ILogger logger)
        {
            _configuration = configuration;
            _logger = logger;

            // Read setting in separate service class
            string dbKeyName = "TestApp:DB:ConnectionString";
            _connectionString = _configuration[dbKeyName];
        }

        #endregion

        public async Task SendNotifications(string sendGridAPIKey)
        {
            try
            {
                await using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var query = "SELECT Email FROM [dbo].[Customer]";

                    // TODO: Send Emails using sendGridAPIKey


                    _logger.LogInformation($"Notifications sent.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("SendNotifications Exception", ex);
                throw;
            }
        }
    }
}
