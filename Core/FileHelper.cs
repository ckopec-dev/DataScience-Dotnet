
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

        public static string ConvertFileSizeToString(int sizeInBytes)
        {
            decimal val = Convert.ToDecimal(sizeInBytes);
            string suffix;

            if (sizeInBytes < 1024)
            {
                return sizeInBytes.ToString() + " bytes";
            }
            else if (sizeInBytes >= 1024 && sizeInBytes < 1048576)
            {
                val /= 1024m;
                suffix = "Kb";
            }
            else
            {
                val /= 1048776m;
                suffix = "Mb";
            }

            return Math.Round(val, 2).ToString() + " " + suffix;
        }
    }
}
