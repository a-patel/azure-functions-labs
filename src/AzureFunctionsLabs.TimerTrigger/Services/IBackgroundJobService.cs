﻿#region Imports
using System.Threading.Tasks;
#endregion

namespace AzureFunctionsLabs.TimerTrigger.Services
{
    public interface IBackgroundJobService
    {
        Task RemoveLogs();
    }
}
