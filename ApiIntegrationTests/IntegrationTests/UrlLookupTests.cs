using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using UrlLookupApi.Models;

namespace ApiTests.IntegrationTests
{
    [TestClass]
    public class UrlLookupTests
    {
        private const string BaseUrl = "urllookupapi.azurewebsites.net/urlLookup";
        private HttpClient _httpClient = new HttpClient();

        [TestMethod]
        public async Task PingRequest_ReturnsPingInResponse()
        {
            var resp = await GetUrlLookupAsync("foo.com", ServiceType.Ping);
            Assert.AreEqual(resp.ipOrDomain, "foo.com");
            Assert.IsNotNull(resp.results.ping);
        }

        private async Task<dynamic> GetUrlLookupAsync(string ipOrDomain, params ServiceType[] services)
        {
            var resp = await _httpClient.GetAsync($"{BaseUrl}?IpOrDomain={ipOrDomain}&Services={string.Join(",", services)}");
            var respStr = await resp.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<dynamic>(respStr);
        }
    }
}
