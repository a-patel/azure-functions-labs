#region Imports
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
#endregion

namespace AzureFunctionsLabs.BlobTrigger
{
    public static partial class BlobFunctions
    {
        [FunctionName("BlogTrigger")]
        public static void Run([BlobTrigger("media-files/{name}", Connection = "StorageConnectionString")]Stream myBlob, string name, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
        }
    }
}



#region @@Reference
/*

 
*/
#endregion
