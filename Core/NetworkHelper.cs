
namespace Core
{
    public static class NetworkHelper
    {
        public static async Task<string> GetHtmlAsync(string url)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Core");

            var content = await client.GetStringAsync(url);

            return content;
        }
    }
}
