
using Core.Salesforce;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using UnitTests.Settings;

namespace UnitTests
{
    [TestClass]
    public class SalesforceHelperUnitTests
    {
        readonly RestClient client = new();
        static SalesforceSettings Settings
        {
            get
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json")
                 .AddUserSecrets<Program>()
                 .Build();

                var appSettings = configuration.GetSection("AppSettings").Get<AppSettings>();
                if (appSettings == null || appSettings.SalesforceSettings == null)
                {
                    throw new InvalidOperationException("AppSettings or SalesforceSettings is not configured properly.");
                }       
                return appSettings.SalesforceSettings;
            }
        }

        [TestInitialize]
        public void TestInitialize()
        {
        }

        private void LoginHelper()
        {
            client.Login(
                Settings.Domain,
                Settings.ClientId,
                Settings.ClientSecret,
                Settings.Username,
                Settings.Password);
        }

        [TestMethod]
        public void LoginTest()
        {
            bool result = client.Login(
                Settings.Domain,
                Settings.ClientId,
                Settings.ClientSecret,
                Settings.Username,
                Settings.Password);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void VersionsTest()
        {
            List<Core.Salesforce.Version> versions = client.Versions(Settings.Domain);

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
