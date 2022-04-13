#region Imports
using AzureFunctionsLabs.HTTPTrigger.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stripe;
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
        private readonly string _stripeWebhookSecret;

        #endregion

        #region Ctor

        public WebhookService(IHttpClientFactory httpClientFactory, IOptions<CloudflareOptions> cloudflareOptions, ILogger logger)
        {
            _client = httpClientFactory.CreateClient();
            _logger = logger;
            _cloudflareSettings = cloudflareOptions.Value;
            _cloudflareUrl = "https://api.cloudflare.com";
            _stripeWebhookSecret = Environment.GetEnvironmentVariable("StripeWebhookSecret");
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


        public async Task<bool> HandleStripeWebhook(string jsonData, string stripeSignature)
        {
            try
            {
                // validate webhook called by stripe only
                var stripeEvent = EventUtility.ConstructEvent(jsonData, stripeSignature, _stripeWebhookSecret);

                switch (stripeEvent.Type)
                {
                    case "customer.created":
                        var customer = stripeEvent.Data.Object as Customer;
                        // do work

                        break;

                    case "customer.subscription.created":
                    case "customer.subscription.updated":
                    case "customer.subscription.deleted":
                    case "customer.subscription.trial_will_end":
                        var subscription = stripeEvent.Data.Object as Subscription;
                        // do work

                        break;

                    case "invoice.created":
                        var newinvoice = stripeEvent.Data.Object as Invoice;
                        // do work

                        break;

                    case "invoice.upcoming":
                    case "invoice.payment_succeeded":
                    case "invoice.payment_failed":
                        var invoice = stripeEvent.Data.Object as Invoice;
                        // do work

                        break;

                    case "coupon.created":
                    case "coupon.updated":
                    case "coupon.deleted":
                        var coupon = stripeEvent.Data.Object as Coupon;
                        // do work

                        break;

                        // DO SAME FOR OTHER EVENTS
                }

                return true;
            }
            catch (StripeException ex)
            {
                _logger.LogError(ex, $"StripWebhook Exception: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"StripWebhook Exception: {ex.Message}");
                return false;
            }
        }
    }
}



#region @@Reference
/*
https://andrewlock.net/creating-my-first-azure-functions-v2-app-a-webhook-and-a-timer/
 
*/
#endregion
