#region Imports
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

#endregion

namespace AzureFunctionsLabs.TimerTrigger.Services
{
    public class WebhookService : IWebhookService
    {
        #region Members

        private readonly HttpClient _client;
        private readonly ILogger _logger;
        private readonly string _webHookUrl;

        #endregion

        #region Ctor

        public WebhookService(IHttpClientFactory httpClientFactory, ILogger logger)
        {
            _client = httpClientFactory.CreateClient();
            _logger = logger;
            _webHookUrl = Environment.GetEnvironmentVariable("WebHookUrl");
        }

        #endregion

        public async Task CallWebhook()
        {
            try
            {
                // READ DATA FROM DATABASE AND POST
                await _client.PostAsJsonAsync(_webHookUrl, "{}");
            }
            catch (Exception ex)
            {
                _logger.LogError("CallWebhook Exception", ex);
                throw;
            }
        }
    }
}



#region @@Reference
/*
https://andrewlock.net/creating-my-first-azure-functions-v2-app-a-webhook-and-a-timer/
 
*/
#endregion
