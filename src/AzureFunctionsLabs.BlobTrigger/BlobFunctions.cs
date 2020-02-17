#region Imports
using AzureFunctionsLabs.BlobTrigger.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.IO;
#endregion

namespace AzureFunctionsLabs.BlobTrigger
{
    public class BlobFunctions
    {
        #region Members

        private readonly IMediaService _mediaService;

        #endregion

        #region Ctor

        public BlobFunctions(IMediaService mediaService)
        {
            _mediaService = mediaService;
        }

        #endregion

        #region Functions


        [FunctionName("BlogTrigger")]
        public static void Run([BlobTrigger("media-files/{name}", Connection = "StorageConnectionString")]Stream myBlob, string name, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
        }

        #endregion

        #region Utilities

        #endregion
    }
}
