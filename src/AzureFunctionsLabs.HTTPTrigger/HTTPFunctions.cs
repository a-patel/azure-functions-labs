#region Imports
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
#endregion

namespace AzureFunctionsLabs.HTTPTrigger
{
    public static partial class HTTPFunctions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName("HTTPTrigger")]
        public static async Task<IActionResult> HTTPTrigger(
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


        /// <summary>
        /// http://<your-function-url>/api/function-name?page=1&orderby=name
        /// </summary>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName("AccessQueryString")]
        public static IActionResult AccessQueryString([HttpTrigger(AuthorizationLevel.Anonymous, "GET")] HttpRequest req, ILogger log)
        {
            log.LogInformation("101 Azure Function Demo - Accessing the query string values in HTTP Triggers");

            // Retrieve query parameters
            IEnumerable<KeyValuePair<string, string>> values = req.GetQueryParameterDictionary();
           
            // From: HttpRequestMessage 
            //IEnumerable<KeyValuePair<string, string>> values = req.GetQueryNameValuePairs();

            // Write query parameters to log
            foreach (KeyValuePair<string, string> val in values)
            {
                log.LogInformation($"Parameter: {val.Key}\nValue: {val.Value}\n");
            }

            // return query parameters in response
            return new OkObjectResult(values);
        }


        /// <summary>
        /// {
        ///    "firstname" : "Ashish Patel",
        ///    "isdisabled" : "true"
        /// }
        /// </summary>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName("ReadingRequestBody")]
        public static async Task<IActionResult> ReadingRequestBody([HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("101 Azure Function Demo - Reading the request body in HTTP Triggers");

            // Read body

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Customer data = JsonConvert.DeserializeObject<Customer>(requestBody);

            // From: HttpRequestMessage 
            //Customer data = await req.Content.ReadAsAsync<Customer>();

            // Echo request data back in the response
            return new OkObjectResult(data);

            // From: HttpRequestMessage 
            //return req.CreateResponse(HttpStatusCode.OK, data);
        }


        /// <summary>
        /// http://&lt;your-function-url&gt;/api/function-name?code=<your-key>
        /// </summary>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName("SecuredFunction")]
        public static IActionResult SecuredFunction([HttpTrigger(AuthorizationLevel.Function, "GET")] HttpRequest req, ILogger log)
        {
            log.LogInformation("Azure Function Demos - Restricting HTTP verbs");

            return new OkObjectResult("Secured response");
        }
    }



    #region Classes

    public class Customer
    {
        public string FirstName { get; set; }
        public bool IsDisabled { get; set; }
    }

    #endregion
}



#region @@Reference
/*
https://docs.microsoft.com/en-us/sandbox/functions-recipes/http-and-webhooks
 
*/
#endregion
