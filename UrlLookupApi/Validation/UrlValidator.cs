using System.Text.RegularExpressions;

namespace UrlLookupApi.Validation
{
    public static class UrlValidator
    {
        private const string _ipRegex = @"^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)(\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)){3}$";
        private const string _domainRegex = "^((?!-)[A-Za-z0-9-]{1,63}(?<!-)\\.)+[A-Za-z]{2,6}$";

        public static bool IsValidIpOrDomain(string ipOrDomain) =>
            IsIpAddress(ipOrDomain) || IsDomain(ipOrDomain);

        public static bool IsIpAddress(string ipOrDomain) =>
            Regex.IsMatch(ipOrDomain, _ipRegex);

        public static bool IsDomain(string ipOrDomain) =>
            Regex.IsMatch(ipOrDomain, _domainRegex);
    }
}
