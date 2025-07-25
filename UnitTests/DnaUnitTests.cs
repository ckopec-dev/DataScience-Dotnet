﻿using Core.Bioinformatics;

namespace UnitTests
{
    [TestClass]
    public class DnaUnitTests
    {
        [TestMethod]
        public void TestInvalidInitialization()
        {
            Assert.ThrowsExactly<InvalidNucleotideException>(() => _ = _ = new Dna("not valid dna"));
        }

        [TestMethod]
        public void TestReverseCompliment()
        {
            // Test plain constructor
            _ = new Dna(); 

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

        [TestMethod]
        public void TestToRna()
        {
            Dna dna = new("ATCG");
            Rna rna = dna.ToRna();

            Assert.AreEqual("AUCG", rna.Code);
        }

        [TestMethod]
        public void TestNucleotides()
        {
            Dna dna = new("ATCG");
            
            Assert.AreEqual(4, dna.Nucleotides.Length);
        }

        [TestMethod]
        public void TestToString()
        {
            Dna dna = new("ATCG");

            Assert.AreEqual("ATCG", dna.ToString());
        }

        [TestMethod]
        public void TestNucleotideCounts()
        {
            Dna dna = new("ATCG");

            Assert.AreEqual(1, dna.NucleotideCounts['A']);
        }

        [TestMethod]
        public void TestGcContent()
        {
            Dna dna = new("AGCTATAG");

            Assert.AreEqual(37.5m, dna.GcContent, 0.1m);
        }
    }
}
