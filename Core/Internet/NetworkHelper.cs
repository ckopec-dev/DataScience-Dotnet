
namespace Core.Internet
{
    public static class NetworkHelper
    {
        private static readonly Random _Random = new();

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

        public static void RandomSleep(int minMilliseconds, int maxMilliseconds)
        {
            if (maxMilliseconds <= minMilliseconds) return;
            int r = _Random.Next(minMilliseconds, maxMilliseconds);
            Thread.Sleep(r);
        }

        public static uint SwapEndianness(ulong x)
        {
            return (uint)(((x & 0x000000ff) << 24) +
                          ((x & 0x0000ff00) << 8) +
                          ((x & 0x00ff0000) >> 8) +
                          ((x & 0xff000000) >> 24));
        }
    }
}
