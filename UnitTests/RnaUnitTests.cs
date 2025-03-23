using Core.Bioinformatics;

namespace UnitTests
{
    [TestClass]
    public class RnaUnitTests
    {
        [TestMethod]
        public void TestInvalidInitialization()
        {
            Assert.ThrowsExactly<InvalidNucleotideException>(() => _ = _ = new Rna("not valid rna"));
        }
    }
}
