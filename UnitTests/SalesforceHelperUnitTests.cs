
using Core.Salesforce;

namespace UnitTests
{
    [TestClass]
    public class SalesforceHelperUnitTests
    {
        readonly RestClient client = new();
        private TestContext? testContextInstance;

        public TestContext? TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

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
            
            Assert.AreEqual(result, true);
        }

        [TestMethod]
        public void VersionsTest()
        {
            List<Core.Salesforce.Version> versions = client.Versions(Secrets.SalesforceDomain);

            Assert.AreNotEqual(versions.Count, 0);
        }

        [TestMethod]
        public void ResourcesTest()
        {
            LoginHelper();

            var resources = client.Resources();

            Assert.AreNotEqual(resources, null);
        }
    }
}
