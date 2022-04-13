#region Import
using FluentFTP;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
#endregion

namespace AzureFunctionsLabs.FTP
{
    public class FTPFunctions
    {
        #region Members

        private readonly IConfiguration _configuration;

        private const string UPLOAD_SCHEDULE = "0 30 1 * * *";

        #endregion

        #region Ctor

        public FTPFunctions(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #endregion

        #region Functions

        [FunctionName("UploadFiles")]
        public async Task UploadFiles([TimerTrigger(UPLOAD_SCHEDULE)] TimerInfo timerInfo, ILogger logger)
        {
            logger.LogInformation($"UploadFiles executed at: {DateTime.Now}");


            string host = _configuration["FTPConfig:Host"];
            string username = _configuration["FTPConfig:Username"];
            string password = _configuration["FTPConfig:Password"];

            #region FTP

            // create an FTP client and specify the host, username and password
            FtpClient client = new FtpClient(host, username, password);

            // connect to the server and automatically detect working FTP settings
            await client.AutoConnectAsync();

            // get a list of files and directories in the "/docs" folder
            foreach (FtpListItem item in await client.GetListingAsync("/docs"))
            {
                // if this is a file
                if (item.Type == FtpFileSystemObjectType.File)
                {
                    // get the file size
                    long size = await client.GetFileSizeAsync(item.FullName);

                    // calculate a hash for the file on the server side (default algorithm)
                    FtpHash hash = await client.GetChecksumAsync(item.FullName);
                }

                // get modified date/time of the file or folder
                DateTime time = await client.GetModifiedTimeAsync(item.FullName);
            }

            // upload a file
            await client.UploadFileAsync(@"C:\MyVideo.mp4", "/docs/MyVideo.mp4");
            //await client.UploadAsync(fileStream: stream, "/docs/MyVideo.mp4");
            //await client.UploadAsync(byte: byte[], "/docs/MyVideo.mp4");

            // move the uploaded file
            await client.MoveFileAsync("/docs/MyVideo.mp4", "/docs/MyVideo_2.mp4");

            // download the file again
            await client.DownloadFileAsync(@"C:\MyVideo_2.mp4", "/docs/MyVideo_2.mp4");
            //await client.DownloadAsync(outStream: stream, "/docs/MyVideo_2.mp4");

            // compare the downloaded file with the server
            if (await client.CompareFileAsync(@"C:\MyVideo_2.mp4", "/docs/MyVideo_2.mp4") == FtpCompareResult.Equal) { }

            // delete the file
            await client.DeleteFileAsync("/docs/MyVideo_2.mp4");

            #endregion


            logger.LogInformation($"UploadFiles execution finished at: {DateTime.Now}");
        }

        #endregion
    }
}



// https://github.com/robinrodricks/FluentFTP
// https://github.com/robinrodricks/FluentFTP/wiki/Quick-Start-Example
