using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UrlLookupApi.Models
{
    public class UrlLookupRequest
    {
        private static readonly List<ServiceType> _defaultServices = new List<ServiceType> { ServiceType.RDAP };

        [Required]
        public string IpOrDomain { get; set; }
        public IEnumerable<ServiceType> Services { get; set; } = _defaultServices;
    }
}
