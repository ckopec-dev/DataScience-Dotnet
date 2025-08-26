using Core;

namespace UnitTests
{
    [TestClass]
    public class CompressionHelperTests
    {
        [TestMethod]
        public void CreateAppendExtractTest()
        {
            DirectoryInfo diInput = Directory.CreateTempSubdirectory();
            DirectoryInfo diOutput = Directory.CreateTempSubdirectory();
            string outputPath = Path.Combine(diOutput.FullName, "test.zip");

            CompressionHelper.CreateZip(diInput.FullName, outputPath);
            bool fileExists = File.Exists(outputPath);
            Assert.IsTrue(fileExists);

            string fileToAppendPath = Path.Combine(diInput.FullName, "append.txt");
            File.Create(fileToAppendPath);
            CompressionHelper.AppendZip(outputPath, fileToAppendPath);
            fileExists = File.Exists(outputPath);
            Assert.IsTrue(fileExists);

            DirectoryInfo diExtract = Directory.CreateTempSubdirectory();
            CompressionHelper.ExtractZip(outputPath, diExtract.FullName);
            int dirs = diExtract.EnumerateDirectories().Count();
            Assert.AreEqual(1, dirs);
        }
    }
}
