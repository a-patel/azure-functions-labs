#region Imports
using AzureFunctionsLabs.BlobTrigger.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.IO;
using ImageResizer;
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
        public void Run([BlobTrigger("media-files/{name}", Connection = "StorageConnectionString")]Stream myBlob, string name, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
        }


        [FunctionName("GenerateThumb")]
        public void GenerateThumb(
            [BlobTrigger("originals/{name}", Connection = "StorageConnectionString")]Stream image,
            [Blob("thumbs/s-{name}", FileAccess.Write, Connection = "StorageConnectionString")]Stream imageSmall,
            ILogger log)
        {
            var instructions = new Instructions
            {
                Height = 320,
                Width = 200,
                Mode = FitMode.Carve,
                Scale = ScaleMode.Both
            };

            ImageBuilder.Current.Build(new ImageJob(image, imageSmall, instructions));

            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{image} \n Size: {image.Length} Bytes");
        }
        #endregion

        #region Utilities

        #endregion
    }
}



#region @@Reference
/*
https://microsoft.github.io/AzureTipsAndTricks/blog/tip158.html
 
*/
#endregion
