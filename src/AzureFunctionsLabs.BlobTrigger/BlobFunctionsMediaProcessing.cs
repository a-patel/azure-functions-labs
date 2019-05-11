#region Imports
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.IO;
#endregion

namespace AzureFunctionsLabs.BlobTrigger
{
    public static partial class BlobFunctions
    {
        [FunctionName("ResizeImage")]
        public static void ResizeImage(Stream inputImage, string imageName, Stream resizedImage, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{imageName} \n Size: {inputImage.Length} Bytes");

            var settings = new ImageResizer.ResizeSettings
            {
                MaxWidth = 400,
                Format = "png"
            };

            ImageResizer.ImageBuilder.Current.Build(inputImage, resizedImage, settings);
        }
    }
}



#region @@Reference
/*
http://jameschambers.com/2016/11/Resizing-Images-Using-Azure-Functions/
 
*/
#endregion
