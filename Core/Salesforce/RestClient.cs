
using System.Text.Json;

namespace Core.Salesforce
{
    public class RestClient()
    {
        readonly HttpClient _HttpClient = new();
        AuthToken? _AuthToken = null;
        
        public AuthToken? AuthToken { get { return _AuthToken; } }
        
        public bool Login(string domain, string clientId, string clientSecret, string username, string password)
        {
            string endpoint = String.Format("https://{0}.my.salesforce.com/services/oauth2/token", domain);

            HttpContent content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                  {"grant_type", "password"},
                  {"client_id", clientId},
                  {"client_secret", clientSecret},
                  {"username", username},
                  {"password", password}
            });

            HttpResponseMessage message = _HttpClient.PostAsync(endpoint, content).Result;
            string response = message.Content.ReadAsStringAsync().Result;

            var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(response);
            if (dict != null)
            {
                _AuthToken = new(dict["access_token"], dict["instance_url"]);

                return true;
            }

            return false;
        }

        public List<Version> Versions(string domain)
        {
            string endpoint = String.Format("https://{0}.my.salesforce.com/services/data/", domain);

            HttpResponseMessage message = _HttpClient.GetAsync(endpoint).Result;
            string response = message.Content.ReadAsStringAsync().Result;

            var dict = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(response);
            List<Version> list = [];

            if (dict != null)
            {
                foreach(var item in dict)
                {
                    list.Add(new Version(item["label"], item["url"], Convert.ToDecimal(item["version"])));
                }
            }

            return list;
        }

        private void AddCompressRequestHeader(HttpRequestMessage request)
        {
            request.Headers.Add("Content_Encoding", "gzip");
        }

        private void AddCompressResponseHeader(HttpRequestMessage request)
        {
            request.Headers.Add("Accept-Encoding", "gzip");
        }
    }
}

