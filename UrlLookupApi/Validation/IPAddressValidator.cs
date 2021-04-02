using System.Text.RegularExpressions;

namespace UrlLookupApi.Validation
{
    public static class IPAddressValidator
    {
        private const string _ipRegex = "^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5]).){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$";

        public static bool IsIpAddress(string ipOrDomain) =>
            Regex.IsMatch(ipOrDomain, _ipRegex);
    }
}
