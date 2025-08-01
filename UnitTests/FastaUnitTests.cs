﻿using Core.Bioinformatics;

namespace UnitTests
{
    [TestClass]
    public class FastaUnitTests
    {
        [TestMethod]
        public void TestCreate()
        {
            string input = ">Rosalind_6404\r\nCCTGCGGAAGATCGGCACTAGAATAGCCAGAACCGTTTCTCTGAGGCTTCCGGCCTTCCC\r\nTCCCACTAATAATTCTGAGG\r\n";
            input += ">Rosalind_6405\r\nTCTGCGGAAGATCGGCACTAGAATAGCCAGAACCGTTTCTCTGAGGCTTCCGGCCTTCCC\r\nTCCCACTAATAATTCTGAGG\r\n";

            Fasta f = new(input);

            if (f.RawInput != null)
            {
                Assert.AreEqual(input, f.RawInput);
            }

            Assert.IsNotNull(f.Entries);

            string output = f.Entries[0].ToString();
            Assert.AreEqual("Rosalind_6404" + Environment.NewLine + "CCTGCGGAAGATCGGCACTAGAATAGCCAGAACCGTTTCTCTGAGGCTTCCGGCCTTCCCTCCCACTAATAATTCTGAGG", output);

            f = new("");
            Assert.AreEqual(0, f.Entries.Count);
        }
    }
}
