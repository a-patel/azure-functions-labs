#region Import
using AzureFunctionsLabs.AzureAppConfiguration.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
#endregion

namespace AzureFunctionsLabs.AzureAppConfiguration
{
    public class DemoFunctions
    {
        #region Members

        private readonly IConfiguration _configuration;
        private readonly INotificationService _notificationService;

        private const string NOTIFICATIONS_SCHEDULE = "0 30 1 * * *";

        #endregion

        #region Ctor

        public DemoFunctions(IConfiguration configuration, INotificationService notificationService)
        {
            _configuration = configuration;
            _notificationService = notificationService;
        }

        #endregion

        #region Functions

        [FunctionName("SendNotifications")]
        public async Task SendNotifications([TimerTrigger(NOTIFICATIONS_SCHEDULE)] TimerInfo timerInfo, ILogger logger)
        {
            logger.LogInformation($"Last timer scheduled = {timerInfo.ScheduleStatus.Last}");


            logger.LogInformation($"SendNotifications executed at: {DateTime.Now}");


            string dbKeyName = "TestApp:DB:ConnectionString";
            string dbConnectionString = _configuration[dbKeyName];

            // TODO: Make Database Connection


            string sendGridKeyName = "TestApp:SendGridAPIKey";
            string sendGridAPIKey = _configuration[sendGridKeyName];

            // TODO: Send Notifications
            await _notificationService.SendNotifications(sendGridAPIKey);

            logger.LogInformation($"SendNotifications execution finished at: {DateTime.Now}");


            logger.LogInformation($"Next timer schedule = {timerInfo.ScheduleStatus.Next}");
        }

        #endregion
    }
}
