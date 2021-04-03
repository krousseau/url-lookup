using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using UrlLookupApi.Models;
using UrlLookupApi.Validation;

namespace UrlLookupApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UrlLookupController : ControllerBase
    {
        private readonly ILogger<UrlLookupController> _logger;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public UrlLookupController(
            ILogger<UrlLookupController> logger,
            IConfiguration configuration,
            HttpClient httpClient
        )
        {
            _logger = logger;
            _configuration = configuration;
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] UrlLookupRequest lookupRequest)
        {
            _logger.LogDebug("Request made to UrlLookup/ w/ Url={url} and Services={services}",
                lookupRequest.IpOrDomain, string.Join(", ", lookupRequest.Services));

            if (!UrlValidator.IsValidIpOrDomain(lookupRequest.IpOrDomain))
            {
                return new BadRequestResult();
            }

            var jobs = GetJobsToRun(lookupRequest);
            var results = await Task.WhenAll(jobs);
            var serviceResults = await BuildResponseAsync(results);

            var response = new
            {
                lookupRequest.IpOrDomain,
                Results = serviceResults
            };

            return new JsonResult(response);
        }

        private static async Task<Dictionary<string, dynamic>> BuildResponseAsync(JobTypeWithResponse[] responses)
        {
            var result = new Dictionary<string, dynamic>();

            foreach (var resp in responses)
            {
                var respStr = await resp.Response.Content.ReadAsStringAsync();
                result.Add(resp.Service.ToString().ToLower(), JsonSerializer.Deserialize<dynamic>(respStr));
            }

            return result;
        }

        private IEnumerable<Task<JobTypeWithResponse>> GetJobsToRun(UrlLookupRequest lookupRequest)
        {
            var requestType = UrlValidator.IsIpAddress(lookupRequest.IpOrDomain) ? "ipaddress" : "domain";
            return lookupRequest.Services.Select(service => RequestDataAsync(lookupRequest.IpOrDomain, service, requestType));
        }

        private async Task<JobTypeWithResponse> RequestDataAsync(string ipOrDomain, ServiceType serviceType, string requestType)
        {
            var funcBaseUrl = _configuration.GetValue<string>("FunctionBaseUrl");

            _logger.LogDebug("Request made to {Service}", serviceType);
            var responseMessage = await _httpClient.GetAsync($"{funcBaseUrl}{serviceType}?ipOrDomain={ipOrDomain}&requestType={requestType}");
            _logger.LogDebug("Response received from {Service}", serviceType);

            return new JobTypeWithResponse(serviceType, responseMessage);
        }

        private class JobTypeWithResponse
        {
            public JobTypeWithResponse(ServiceType service, HttpResponseMessage response)
            {
                Service = service;
                Response = response;
            }

            public ServiceType Service { get; set; }
            public HttpResponseMessage Response { get; set; }
        }
    }
}
