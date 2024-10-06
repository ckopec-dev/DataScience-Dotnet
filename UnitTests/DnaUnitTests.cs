
using Core.Bioinformatics;

namespace UnitTests
{
    [TestClass]
    public class DnaUnitTests
    {
        [TestMethod]
        public void TestValidInitialization()
        {
            Dna dna = new("GATGGAACTTGACTACGTAAATT");
            Assert.IsNotNull(dna);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidInitialization()
        {
            Dna dna = new("not valid dna");
        }
    }
}
