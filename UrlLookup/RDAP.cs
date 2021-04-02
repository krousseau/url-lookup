using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace UrlLookup
{
    public static class RDAP
    {
        private const string ServiceBaseUrl = "https://rdap.org/";

        [FunctionName("RDAP")]
        public static async Task<string> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            string url = req.Query["ipOrDomain"];
            string type = req.Query["requestType"];
            log.LogInformation("Performing RDAP {LookupType} lookup on {Url}", type, url);

            using var httpClient = new HttpClient();
            var rdapResp = await httpClient.GetAsync($"{ServiceBaseUrl}{type}/{url}");
            return await rdapResp.Content.ReadAsStringAsync();
        }
    }
}
