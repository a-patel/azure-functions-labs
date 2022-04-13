#region Imports
using System.Threading.Tasks;
#endregion

namespace AzureFunctionsLabs.AzureAppConfiguration.Services
{
    public interface INotificationService
    {
        Task SendNotifications(string sendGridAPIKey);
    }
}
