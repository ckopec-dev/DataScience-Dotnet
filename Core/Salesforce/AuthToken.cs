
namespace Core.Salesforce
{
    public class AuthToken(string token, string instanceUrl)
    {
        public string Token { get; set; } = token;
        public string InstanceUrl { get; set; } = instanceUrl;
    }
}
