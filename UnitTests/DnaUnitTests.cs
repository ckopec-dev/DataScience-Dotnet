
using Core.Bioinformatics;

namespace UnitTests
{
    [TestClass]
    public class DnaUnitTests
    {
        [TestMethod]
        public void TestValidInitialization()
        {
            /*
            Dna dna1 = new();
            Assert.IsNotNull(dna1);

            Dna dna2 = new("GATGGAACTTGACTACGTAAATT");
            Assert.IsNotNull(dna2.Code);
            
            Assert.AreEqual("GATGGAACTTGACTACGTAAATT", dna2.ToString());

            Rna rna = dna2.ToRna();
            Assert.IsNotNull(rna);

            //dna = new Dna("ATCG");
            //Assert.AreEqual("TAGC", dna.ToString());
            */
        }

        [TestMethod]
        public void TestInvalidInitialization()
        {
            Assert.ThrowsExactly<InvalidNucleotideException>(() => _ = _ = new Dna("not valid dna"));
        }

        [TestMethod]
        public void TestReverseCompliment()
        {
            Dna dna = new("ATCG");
            Assert.AreEqual("CGAT", dna.ReverseCompliment);
        }

        [TestMethod]
        public void TestHammingDistance()
        {
            Dna dna1 = new("GAGCCTACTAACGGGAT");
            Dna dna2 = new("CATCGTAATGACGGCCT");

            Assert.AreEqual(7, Dna.HammingDistance(dna1, dna2));
        }

        [TestMethod]
        public void TestHammingDistanceException()
        {
            Dna dna1 = new("GAGCCTACT");
            Dna dna2 = new("CATCGTAATGACGGCCT");

            Assert.ThrowsExactly<InvalidComparisonException>(() => _ = Dna.HammingDistance(dna1, dna2));
        }
    }
}
