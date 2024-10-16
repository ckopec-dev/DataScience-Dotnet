
using Core.Salesforce;

namespace UnitTests
{
    [TestClass]
    public class SalesforceHelperUnitTests
    {
        RestClient client = new();
        
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
    }
}
