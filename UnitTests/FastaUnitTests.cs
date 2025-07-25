using Core.Bioinformatics;

namespace UnitTests
{
    [TestClass]
    public class FastaUnitTests
    {
        [TestMethod]
        public void TestCreate()
        {
            string input = ">Rosalind_6404\r\nCCTGCGGAAGATCGGCACTAGAATAGCCAGAACCGTTTCTCTGAGGCTTCCGGCCTTCCC\r\nTCCCACTAATAATTCTGAGG";

            Fasta f = new(input);

            if (f.RawInput != null)
            {
                Assert.AreEqual(input, f.RawInput);
            }

            Assert.IsNotNull(f.Entries);

            f = new("");
            Assert.AreEqual(0, f.Entries.Count);
        }
    }
}
