using Core.Bioinformatics;

namespace UnitTests
{
    [TestClass]
    public class RnaUnitTests
    {
        [TestMethod]
        public void TestInvalidInitialization()
        {
            _ = new Rna();

            Assert.ThrowsExactly<InvalidNucleotideException>(() => _ = _ = new Rna("not valid rna"));
        }

        [TestMethod]
        public void TestToString()
        {
            Rna rna = new("CAGU");

            Assert.AreEqual("CAGU", rna.ToString());
        }

        [TestMethod]
        public void TestNucleotides()
        {
            Rna rna = new("CAGU");

            Assert.AreEqual(4, rna.Nucleotides.Length);
        }

        [TestMethod]
        public void TestNucleotideCounts()
        {
            Rna rna = new("CAGU");

            Assert.AreEqual(1, rna.NucleotideCounts['A']);
        }
    }
}
