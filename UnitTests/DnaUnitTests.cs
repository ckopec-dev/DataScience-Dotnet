
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
            Assert.ThrowsException<ArgumentException>(() => _ = new Dna("not valid dna"));
        }

        [TestMethod]
        public void TestReverseCompliment()
        {
            Dna dna = new("ATCG");
            Assert.AreEqual("CGAT", dna.ReverseCompliment);
        }
    }
}
