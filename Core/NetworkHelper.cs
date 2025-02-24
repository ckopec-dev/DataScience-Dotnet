
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

        public static string ProjectPath()
        {
            string workingDir = Environment.CurrentDirectory ?? throw new InvalidPathException();
            DirectoryInfo? di = Directory.GetParent(workingDir) ?? throw new InvalidPathException();
            DirectoryInfo? diParent = di.Parent ?? throw new InvalidPathException();
            DirectoryInfo? diGrandparent = diParent.Parent ?? throw new InvalidPathException();
            string projectDirectory = diGrandparent.FullName;
            return projectDirectory;
        }
    }
}
