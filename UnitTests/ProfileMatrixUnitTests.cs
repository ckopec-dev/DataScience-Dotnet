using Core.Bioinformatics;
using System.Text;

namespace UnitTests
{
    [TestClass]
    public class ProfileMatrixUnitTests
    {
        private readonly List<Dna> _dnaValid = [new Dna("ACTG"), new Dna("CCTG")];
        private readonly List<Dna> _dnaInvalid = [new Dna("ACTG"), new Dna("CCT")];
        ProfileMatrix _profileMatrix = null!;

        [TestInitialize]
        public void Setup()
        {
            _profileMatrix = new ProfileMatrix(_dnaValid);
        }

        [TestMethod]
        public void TestToString()
        {
            // See https://rosalind.info/problems/cons/

            StringBuilder sb = new();

            sb.AppendLine("A: 1 0 0 0");
            sb.AppendLine("C: 1 2 0 0");
            sb.AppendLine("G: 0 0 0 2");
            sb.AppendLine("T: 0 0 2 0");

            Assert.AreEqual(sb.ToString(), _profileMatrix.ToString());
        }

        [TestMethod]
        public void TestInvalidComparisonException()
        {
            Assert.ThrowsExactly<InvalidComparisonException>(() => _ = new ProfileMatrix(_dnaInvalid));
        }

        [TestMethod]
        public void TestConsensus()
        {
            Assert.AreEqual("ACTG", _profileMatrix.Consensus);
        }
    }
}
