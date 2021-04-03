using Microsoft.VisualStudio.TestTools.UnitTesting;
using UrlLookupApi.Validation;

namespace ApiTests.UnitTests
{
    [TestClass]
    public class UrlValidatorTests
    {
        [TestMethod]
        public void IsDomain_ReturnsTrue_ForValidDomain()
        {
            var isValid = UrlValidator.IsDomain("foo.co");
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void IsDomain_ReturnsTrue_ForValidSubDomain()
        {
            var isValid = UrlValidator.IsDomain("bar.foo.co");
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void IsDomain_ReturnsFalse_ForDomainWithNoTld()
        {
            var isValid = UrlValidator.IsDomain("foo");
            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void IsDomain_ReturnsFalse_ForDomainWithOneLetterTld()
        {
            var isValid = UrlValidator.IsDomain("foo.x");
            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void IsIpAddress_ReturnsTrue_ForValidIpAddress()
        {
            var isValid = UrlValidator.IsIpAddress("192.168.2.3");
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void IsIpAddress_ReturnsFalse_ForIpAddressWithExtraNumbers()
        {
            var isValid = UrlValidator.IsIpAddress("192.168.2.3.5");
            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void IsIpAddress_ReturnsFalse_ForIpAddressTooShort()
        {
            var isValid = UrlValidator.IsIpAddress("192.168");
            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void IsIpAddress_ReturnsFalse_ForIpAddressWithLetters()
        {
            var isValid = UrlValidator.IsIpAddress("192.168.xc.hi");
            Assert.IsFalse(isValid);
        }
    }
}
