
using System.Text.Json;

namespace Core.Salesforce
{
    public class RestClient(string domain, string clientId, string clientSecret, string username, string password)
    {
        private readonly string _Domain = domain;
        private readonly string _ClientId = clientId;
        private readonly string _ClientSecret = clientSecret;
        private readonly string _Username = username;
        private readonly string _Password = password;
        
        public string AuthEndpoint { get { return String.Format("https://{0}.my.salesforce.com/services/oauth2/token", _Domain); }}

        public AuthToken? Login()
        {
            HttpClient client = new();

            HttpContent content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                  {"grant_type", "password"},
                  {"client_id", _ClientId},
                  {"client_secret", _ClientSecret},
                  {"username", _Username},
                  {"password", _Password}
            });

            HttpResponseMessage message = client.PostAsync(AuthEndpoint, content).Result;
            string response = message.Content.ReadAsStringAsync().Result;

            var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(response);
            if (dict != null)
            {
                return new(dict["access_token"], dict["instance_url"]);
            }

            return null;
        }
    }
}

