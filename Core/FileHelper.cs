
namespace Core
{
    public static class FileHelper
    {
        public static string GetProjectPath()
        {
            string path = Environment.CurrentDirectory;

            DirectoryInfo? di = Directory.GetParent(path);

            if (di != null)
                di = di.Parent;
            if (di != null)
                di = di.Parent;
            if (di != null)
                return di.FullName;
            else
                throw new Exception("Unable to determine project path.");
        }
    }
}
