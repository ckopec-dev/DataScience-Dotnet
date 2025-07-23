using Core.Bioinformatics;

namespace UnitTests
{
    [TestClass]
    public class FastaUnitTests
    {
        [TestMethod]
        public void TestA()
        {
            string input = ">Rosalind_6404\r\nCCTGCGGAAGATCGGCACTAGAATAGCCAGAACCGTTTCTCTGAGGCTTCCGGCCTTCCC\r\nTCCCACTAATAATTCTGAGG";

            Fasta f = new(input);

            if (f.RawInput != null)
            {
                Assert.AreEqual(input, f.RawInput);
            }
        }
    }
}
