
namespace UnitTests.Settings
{
    public class SalesforceSettings(string domain, string clientId, string clientSecret, string username, string password)
    {
        public string Domain { get; set; } = domain;
        public string ClientId { get; set; } = clientId;
        public string ClientSecret { get; set; } = clientSecret;
        public string Username { get; set; } = username;
        public string Password { get; set; } = password;
    }
}
