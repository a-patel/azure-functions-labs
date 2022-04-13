#region Imports
using System.Threading.Tasks;
#endregion

namespace AzureFunctionsLabs.HTTPTrigger.Services
{
    public interface IWebhookService
    {
        Task<bool> ClearCloudflareCache();
    }
}
