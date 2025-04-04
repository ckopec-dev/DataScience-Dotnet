
using System.IO.Compression;

namespace Core
{
    /// <summary>
    /// References:
    /// https://learn.microsoft.com/en-us/dotnet/standard/io/how-to-compress-and-extract-files
    /// </summary>
    public static class CompressionHelper
    {
        public static void CreateZip(string inputPath, string outputPath)
        {
            ZipFile.CreateFromDirectory(inputPath, outputPath);
        }

        public static void ExtractZip(string inputPath, string outputPath)
        {
            ZipFile.ExtractToDirectory(inputPath, outputPath);
        }

        public static void AppendZip(string existingZipPath, string fileToAppendPath)
        {
            using FileStream existingZipToOpen = new(existingZipPath, FileMode.Open);
            using ZipArchive archive = new(existingZipToOpen, ZipArchiveMode.Update);
            archive.CreateEntry(fileToAppendPath);
        }
    }
}
