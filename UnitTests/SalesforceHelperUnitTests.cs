
using Core.Salesforce;

namespace UnitTests
{
    [TestClass]
    public class SalesforceHelperUnitTests
    {
        readonly RestClient client = new();

        private void LoginHelper()
        {
            client.Login(
                Secrets.SalesforceDomain,
                Secrets.SalesforceClientId,
                Secrets.SalesforceClientSecret,
                Secrets.SalesforceUsername,
                Secrets.SalesforcePassword);
        }

        [TestMethod]
        public void LoginTest()
        {
            bool result = client.Login(
                Secrets.SalesforceDomain,
                Secrets.SalesforceClientId,
                Secrets.SalesforceClientSecret,
                Secrets.SalesforceUsername,
                Secrets.SalesforcePassword);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void VersionsTest()
        {
            List<Core.Salesforce.Version> versions = client.Versions(Secrets.SalesforceDomain);

            Assert.AreNotEqual(0, versions.Count);
        }

        [TestMethod]
        public void ResourcesTest()
        {
            LoginHelper();

            var resources = client.Resources();

            Assert.IsNotNull(resources);
        }
    }
}
