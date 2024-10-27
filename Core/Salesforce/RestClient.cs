
using System.Net.Http.Headers;
using System.Text.Json;

namespace Core.Salesforce
{
    public class RestClient()
    {
        public const string DEFAULT_VERSION = "v62.0";
        readonly HttpClient _HttpClient = new();
        AuthToken? _AuthToken = null;
        private string? _Version;

        public string Version
        {
            get
            {
                if (_Version == null)
                    return DEFAULT_VERSION;
                else
                    return "v" + _Version;
            }
            set { _Version = value; }   
        }

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

        public void Resources()
        {
            // curl https://MyDomainName.my.salesforce.com/services/data/v53.0/ -H "Authorization: Bearer
            // access_token" -H "X - PrettyPrint:1"

            if (AuthToken == null)
                throw new NotAuthorizedException();

            string endpoint = String.Format("{0}/{1}/", AuthToken.InstanceUrl, Version);

            Console.WriteLine(endpoint);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, endpoint);
            request.Headers.Add("Authorization", "Bearer " + AuthToken.Token);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage message = _HttpClient.SendAsync(request).Result;
            
            string response = message.Content.ReadAsStringAsync().Result;

            Console.WriteLine(response);
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

