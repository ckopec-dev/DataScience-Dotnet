using Core.Bioinformatics;

namespace UnitTests
{
    [TestClass]
    public class RnaUnitTests
    {
        [TestMethod]
        public void TestValidInitialization()
        {
            Rna rna = new("GAUGGAACUUGACUACGUAAAUU");
            Assert.IsNotNull(rna);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidInitialization()
        {
            _ = new Rna("not valid rna");
        }
    }
}
