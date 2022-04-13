#region Imports
using System.Threading.Tasks;
#endregion

namespace AzureFunctionsLabs.HTTPTrigger.Services
{
    public interface IWebhookService
    {
        Task<bool> ClearCloudflareCache();

        Task<bool> HandleStripeWebhook(string jsonData, string stripeSignature);
    }
}
