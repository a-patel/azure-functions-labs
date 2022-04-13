#region Imports
using AzureFunctionsLabs.HTTPTrigger.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;

#endregion

namespace AzureFunctionsLabs.HTTPTrigger.Services
{
    public class WebhookService : IWebhookService
    {
        #region Members

        private readonly HttpClient _client;
        private readonly ILogger _logger;
        private readonly CloudflareOptions _cloudflareSettings;
        private readonly string _cloudflareUrl;

        #endregion

        #region Ctor

        public WebhookService(IHttpClientFactory httpClientFactory, IOptions<CloudflareOptions> cloudflareOptions, ILogger logger)
        {
            _client = httpClientFactory.CreateClient();
            _logger = logger;
            _cloudflareSettings = cloudflareOptions.Value;
            _cloudflareUrl = "https://api.cloudflare.com";
        }

        #endregion

        public async Task<bool> ClearCloudflareCache()
        {
            _client.BaseAddress = new Uri(_cloudflareUrl);

            var zoneId = _cloudflareSettings.ZoneId;
            var xAuthEmail = _cloudflareSettings.Email;
            var xAuthKey = _cloudflareSettings.AuthKey;

            var path = $"/client/v4/zones/{zoneId}/purge_cache";
            var body = new { purge_everything = true };

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Add("X-Auth-Email", xAuthEmail);
            _client.DefaultRequestHeaders.Add("X-Auth-Key", xAuthKey);

            var response = await _client.PostAsJsonAsync(path, body);

            return response.IsSuccessStatusCode;
        }
    }
}



#region @@Reference
/*
https://andrewlock.net/creating-my-first-azure-functions-v2-app-a-webhook-and-a-timer/
 
*/
#endregion
