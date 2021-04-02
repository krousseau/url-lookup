using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace UrlLookup
{
    public static class GeoIP
    {
        private static string ServiceBaseUrl = "http://ip-api.com/json/";

        [FunctionName("GeoIP")]
        public static async Task<string> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            string url = req.Query["ipOrDomain"];
            log.LogInformation("Performing GeoIP on {Url}", url);

            using var httpClient = new HttpClient();
            var resp = await httpClient.GetAsync($"{ServiceBaseUrl}/{url}");
            return await resp.Content.ReadAsStringAsync();
        }
    }
}

