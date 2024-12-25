
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

        public static void CreateGZ(string directoryPath)
        {
            DirectoryInfo di = new(directoryPath);
            
            foreach (FileInfo fileToCompress in di.GetFiles())
            {
                using FileStream originalFileStream = fileToCompress.OpenRead();
                if ((File.GetAttributes(fileToCompress.FullName) &
                   FileAttributes.Hidden) != FileAttributes.Hidden & fileToCompress.Extension != ".gz")
                {
                    using (FileStream compressedFileStream = File.Create(fileToCompress.FullName + ".gz"))
                    {
                        using GZipStream compressionStream = new(compressedFileStream,
                           CompressionMode.Compress);
                        originalFileStream.CopyTo(compressionStream);
                    }
                    FileInfo info = new(directoryPath + Path.DirectorySeparatorChar + fileToCompress.Name + ".gz");
                }
            }
        }

        public static void ExtractGZ(string filePath)
        {
            FileInfo fi = new(filePath);

            using FileStream originalFileStream = fi.OpenRead();
            string currentFileName = fi.FullName;
            string newFileName = currentFileName.Remove(currentFileName.Length - fi.Extension.Length);

            using FileStream decompressedFileStream = File.Create(newFileName);
            using GZipStream decompressionStream = new(originalFileStream, CompressionMode.Decompress);
            decompressionStream.CopyTo(decompressedFileStream);
        }
    }
}
