// File: Bioinformatics.Tests/BioTests.cs
using Core.Bioinformatics;
using Xunit;
using Assert = Xunit.Assert;

namespace UnitTests
{
    public class BioTests
    {
        [Fact]
        public void BioSequence_ToString_ShowsIdAndSequence()
        {
            var b = new BioSequence("ATGC", "seq1");
            Assert.Equal(">seq1\nATGC", b.ToString());
        }

        [Fact]
        public void BioSequence_Constructor_ThrowsOnNullOrEmpty()
        {
            Assert.Throws<ArgumentNullException>(() => new BioSequence(null));
            Assert.Throws<ArgumentException>(() => new BioSequence(""));
        }

        [Fact]
        public void ValidateDna_AllowsValidAndRejectsInvalid()
        {
            Bio.ValidateDna("ATGCNatgcn");
            var ex = Assert.Throws<ArgumentException>(() => Bio.ValidateDna("ATBG"));
            Assert.Contains("Invalid DNA base", ex.Message);
            Assert.Throws<ArgumentNullException>(() => Bio.ValidateDna(null));
            Assert.Throws<ArgumentException>(() => Bio.ValidateDna(""));
        }

        [Fact]
        public void ReverseComplement_BasicAndEdgeCases()
        {
            Assert.Equal("GCAT", Bio.ReverseComplement("ATGC"));
            Assert.Equal(string.Empty, Bio.ReverseComplement(""));
            Assert.Equal("NN", Bio.ReverseComplement("NN"));
            // lower-case mapping
            Assert.Equal("acgt", Bio.ReverseComplement("acgt").ToLower());
            Assert.Throws<ArgumentNullException>(() => Bio.ReverseComplement(null));
            Assert.Equal("XYZ", Bio.ReverseComplement("ZYX"));
            //Assert.Throws<ArgumentNullException>(() => Bio.ReverseComplement(""));
        }

        [Fact]
        public void Transcribe_ReplacesTwithU()
        {
            Assert.Equal("AUGC", Bio.Transcribe("ATGC"));
            Assert.Equal("AuGC", Bio.Transcribe("AtGC"));
            Assert.Equal(string.Empty, Bio.Transcribe(""));
            Assert.Throws<ArgumentNullException>(() => Bio.Transcribe(null));
        }

        [Fact]
        public void GcContent_CalculatesCorrectly_AndErrors()
        {
            Assert.Equal(0.5, Bio.GcContent("ATGC"));
            Assert.Equal(1.0, Bio.GcContent("GGCC"));
            Assert.Equal(0.0, Bio.GcContent("ATAT"));
            Assert.Throws<ArgumentNullException>(() => Bio.GcContent(null));
            Assert.Throws<ArgumentException>(() => Bio.GcContent("")); // empty is invalid
            var ex = Assert.Throws<ArgumentException>(() => Bio.GcContent("ATBX"));
            Assert.Contains("Invalid base", ex.Message);
        }

        [Fact]
        public void KmerFrequencies_Basic()
        {
            var freqs = Bio.KmerFrequencies("ATAT", 2);
            Assert.Equal(2, freqs["AT"]);
            Assert.Equal(1, freqs["TA"]);
            Assert.Throws<ArgumentNullException>(() => Bio.KmerFrequencies(null, 2));
            Assert.Throws<ArgumentOutOfRangeException>(() => Bio.KmerFrequencies("AT", 0));
        }

        [Fact]
        public void HammingAndLevenshtein_Distances()
        {
            Assert.Equal(2, Bio.HammingDistance("GATTACA", "GACTATA"));
            Assert.Throws<ArgumentNullException>(() => Bio.HammingDistance(null, "A"));
            Assert.Throws<ArgumentException>(() => Bio.HammingDistance("A", "AA"));

            Assert.Equal(3, Bio.LevenshteinDistance("kitten", "sitting")); // classic
            Assert.Equal(0, Bio.LevenshteinDistance("", ""));
            Assert.Equal(3, Bio.LevenshteinDistance("", "abc"));
            Assert.Equal(3, Bio.LevenshteinDistance("abc", ""));
            Assert.Throws<ArgumentNullException>(() => Bio.LevenshteinDistance(null, null));
        }
    }
}
