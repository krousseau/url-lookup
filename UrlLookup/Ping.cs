using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using NetworkInfo = System.Net.NetworkInformation;

namespace UrlLookup
{
    public static class Ping
    {
        private static int TenSecondTimeout = 10000;

        [FunctionName("Ping")]
        public static Task<JsonResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            string url = req.Query["ipOrDomain"];
            log.LogInformation("Performing ping on {Url}", url);

            var ping = new NetworkInfo.Ping();
            var reply = ping.Send(url, TenSecondTimeout);

            return Task.FromResult(new JsonResult(new
            {
                IPStatus = reply.Status.ToString(),
                Address = reply.Address.ToString(),
                reply.RoundtripTime
            }));
        }
    }
}

