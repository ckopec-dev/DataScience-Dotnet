using System.Net.Http.Headers;
using System.Text.Json;

namespace Core.Salesforce
{
    /// <summary>
    /// See https://developer.salesforce.com/docs/atlas.en-us.api_rest.meta/api_rest/intro_rest.htm
    /// General url syntax: https://mydomainname.my.salesforce.com/services/data/vXX.X/resource/
    /// </summary>
    public class RestClient()
    {
        readonly HttpClient _HttpClient = new();
        AuthToken? _AuthToken = null;
        private string _VersionUrl = "/services/data/v62.0";
        private bool _CompressRequest = false;
        private bool _CompressResponse = false;

        public string VersionUrl
        {
            get { return _VersionUrl; }
            set { _VersionUrl = value; }   
        }

        public bool CompressRequest
        {
            get { return _CompressRequest; }
            set { _CompressRequest = value; }
        }

        public bool CompressResponse
        {
            get { return _CompressResponse; } 
            set { _CompressResponse = value; }
        }

        public AuthToken? AuthToken { get { return _AuthToken; } }
        
        public bool Login(string domain, string clientId, string clientSecret, string username, string password)
        {
            string endpoint = String.Format("https://{0}.my.salesforce.com/services/oauth2/token", domain);

            Console.WriteLine("Login endpoint: {0}", endpoint);
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
            Console.WriteLine("Login response: {0}", response);

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
            Console.WriteLine("Versions endpoint: {0}", endpoint);

            HttpRequestMessage request = new(HttpMethod.Get, endpoint);
            AddHeaders(request);

            HttpResponseMessage message = _HttpClient.SendAsync(request).Result;

            string response = message.Content.ReadAsStringAsync().Result;
            Console.WriteLine("Versions response: {0}", response);

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

        public Dictionary<string, string>? Resources()
        {
            // curl https://MyDomainName.my.salesforce.com/services/data/v53.0/ -H "Authorization: Bearer
            // access_token" -H "X - PrettyPrint:1"

            if (AuthToken == null)
                throw new NotAuthorizedException();

            string endpoint = String.Format("{0}/{1}/", AuthToken.InstanceUrl, VersionUrl);
            Console.WriteLine("Resources endpoint: {0}", endpoint);
            
            HttpRequestMessage request = new(HttpMethod.Get, endpoint);
            AddHeaders(request);

            HttpResponseMessage message = _HttpClient.SendAsync(request).Result;
            
            string response = message.Content.ReadAsStringAsync().Result;
            Console.WriteLine("Resources response: {0}", response);

            return JsonSerializer.Deserialize<Dictionary<string, string>>(response);
        }

        private void AddHeaders(HttpRequestMessage request)
        {
            if (CompressRequest)
                request.Headers.Add("Content_Encoding", "gzip");
            if (CompressResponse)
                request.Headers.Add("Accept-Encoding", "gzip");

            if (AuthToken != null)
            {
                request.Headers.Add("Authorization", "Bearer " + AuthToken.Token);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
        }
    }
}

