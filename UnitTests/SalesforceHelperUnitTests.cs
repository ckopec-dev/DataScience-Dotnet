
using Core.Salesforce;

namespace UnitTests
{
    [TestClass]
    public class SalesforceHelperUnitTests
    {
        [TestMethod]
        public void LoginTest()
        {
            RestClient client = new();

            bool result = client.Login(
                Secrets.SalesforceDomain,
                Secrets.SalesforceClientId,
                Secrets.SalesforceClientSecret,
                Secrets.SalesforceUsername,
                Secrets.SalesforcePassword);
            
            Assert.AreEqual(result, true);
        }
    }
}
