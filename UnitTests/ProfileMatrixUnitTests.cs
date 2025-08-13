using Core.Bioinformatics;
using System.Text;

namespace UnitTests
{
    [TestClass]
    public class ProfileMatrixUnitTests
    {
        [TestMethod]
        public void TestToString()
        {
            // See https://rosalind.info/problems/cons/

            List<Dna> dna = [new Dna("ACTG"), new Dna("CCTG")];

            ProfileMatrix pm = new(dna);

            StringBuilder sb = new();

            sb.AppendLine("A: 1 0 0 0");
            sb.AppendLine("C: 1 2 0 0");
            sb.AppendLine("G: 0 0 0 2");
            sb.AppendLine("T: 0 0 2 0");

            Assert.AreEqual(sb.ToString(), pm.ToString());
        }

        [TestMethod]
        public void TestInvalidComparisonException()
        {
            List<Dna> dna = [new Dna("ACTG"), new Dna("CCT")];

            Assert.ThrowsExactly<InvalidComparisonException>(() => _ = new ProfileMatrix(dna));
        }
    }
}
