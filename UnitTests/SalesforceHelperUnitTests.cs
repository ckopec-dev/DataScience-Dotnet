
using Core.Salesforce;

namespace UnitTests
{
    [TestClass]
    public class SalesforceHelperUnitTests
    {
        [TestMethod]
        public void LoginTest()
        {
            RestClient client = new(
            Secrets.SalesforceDomain,
            Secrets.SalesforceClientId,
            Secrets.SalesforceClientSecret,
            Secrets.SalesforceUsername,
            Secrets.SalesforcePassword
            );

            AuthToken? result = client.Login();
            Assert.IsNotNull(result);
        }
    }
}
