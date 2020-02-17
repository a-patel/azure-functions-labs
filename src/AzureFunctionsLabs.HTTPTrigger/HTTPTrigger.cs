#region Imports
using AzureFunctionsLabs.HTTPTrigger.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
#endregion

namespace AzureFunctionsLabs.HTTPTrigger
{
    public class HTTPTrigger
    {
        #region Members

        private readonly IWebhookService _webhookService;

        #endregion

        #region Ctor

        public HTTPTrigger(IWebhookService webhookService)
        {
            _webhookService = webhookService;
        }

        #endregion

        #region Functions

        [FunctionName("HTTPTrigger")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            return name != null
                ? (ActionResult)new OkObjectResult($"Hello, {name}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }

        #endregion

        #region Utilities

        #endregion
    }
}



#region @@Reference
/*

 
*/
#endregion
