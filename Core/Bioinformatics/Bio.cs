using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices; // Add this for GeneratedRegexAttribute

namespace Core.Bioinformatics
{
    public static class Bio // Make class partial for source generator
    {
        private static readonly Dictionary<char, char> DnaComplement = new()
        {
            ['A'] = 'T',
            ['T'] = 'A',
            ['G'] = 'C',
            ['C'] = 'G',
            ['a'] = 't',
            ['t'] = 'a',
            ['g'] = 'c',
            ['c'] = 'g',
            // allow ambiguous but map to themselves for simplicity (N->N)
            ['N'] = 'N',
            ['n'] = 'n'
        };

        private static readonly Dictionary<string, char> CodonTable = new(StringComparer.OrdinalIgnoreCase)
        {
            // Minimal standard codon table (stop = '*')
            {"TTT",'F'},{"TTC",'F'},{"TTA",'L'},{"TTG",'L'},
            {"CTT",'L'},{"CTC",'L'},{"CTA",'L'},{"CTG",'L'},
            {"ATT",'I'},{"ATC",'I'},{"ATA",'I'},{"ATG",'M'},
            {"GTT",'V'},{"GTC",'V'},{"GTA",'V'},{"GTG",'V'},

            {"TCT",'S'},{"TCC",'S'},{"TCA",'S'},{"TCG",'S'},
            {"CCT",'P'},{"CCC",'P'},{"CCA",'P'},{"CCG",'P'},
            {"ACT",'T'},{"ACC",'T'},{"ACA",'T'},{"ACG",'T'},
            {"GCT",'A'},{"GCC",'A'},{"GCA",'A'},{"GCG",'A'},

            {"TAT",'Y'},{"TAC",'Y'},{"TAA",'*'},{"TAG",'*'},
            {"CAT",'H'},{"CAC",'H'},{"CAA",'Q'},{"CAG",'Q'},
            {"AAT",'N'},{"AAC",'N'},{"AAA",'K'},{"AAG",'K'},
            {"GAT",'D'},{"GAC",'D'},{"GAA",'E'},{"GAG",'E'},

            {"TGT",'C'},{"TGC",'C'},{"TGA",'*'},{"TGG",'W'},
            {"CGT",'R'},{"CGC",'R'},{"CGA",'R'},{"CGG",'R'},
            {"AGT",'S'},{"AGC",'S'},{"AGA",'R'},{"AGG",'R'},
            {"GGT",'G'},{"GGC",'G'},{"GGA",'G'},{"GGG",'G'}
        };

        private static readonly double[] AminoAcidMass = new double[128]; // ascii-indexed; minimal later if needed
        private static readonly char[] separator = ['\r', '\n'];

        static Bio()
        {
            // populate a few amino-acid average masses (optional; used by ComputeProteinMass if called)
            AminoAcidMass['A'] = 89.09; AminoAcidMass['R'] = 174.20; AminoAcidMass['N'] = 132.12;
            AminoAcidMass['D'] = 133.10; AminoAcidMass['C'] = 121.16; AminoAcidMass['E'] = 147.13;
            AminoAcidMass['Q'] = 146.15; AminoAcidMass['G'] = 75.07; AminoAcidMass['H'] = 155.16;
            AminoAcidMass['I'] = 131.17; AminoAcidMass['L'] = 131.17; AminoAcidMass['K'] = 146.19;
            AminoAcidMass['M'] = 149.21; AminoAcidMass['F'] = 165.19; AminoAcidMass['P'] = 115.13;
            AminoAcidMass['S'] = 105.09; AminoAcidMass['T'] = 119.12; AminoAcidMass['W'] = 204.23;
            AminoAcidMass['Y'] = 181.19; AminoAcidMass['V'] = 117.15;
            AminoAcidMass['*'] = 0.0; // stop codon
        }

        // Validate DNA characters (A,T,G,C,N - case-insensitive)
        public static void ValidateDna(string? seq)
        {
            ArgumentNullException.ThrowIfNull(seq);
            if (seq.Length == 0) throw new ArgumentException("Sequence must not be empty.", nameof(seq));
            foreach (char c in seq)
            {
                if (!(c == 'A' || c == 'T' || c == 'G' || c == 'C' || c == 'N'
                      || c == 'a' || c == 't' || c == 'g' || c == 'c' || c == 'n'))
                {
                    throw new ArgumentException($"Invalid DNA base: '{c}'");
                }
            }
        }

        public static string ReverseComplement(string? dna)
        {
            ArgumentNullException.ThrowIfNull(dna);
            if (dna.Length == 0) return string.Empty;
            var sb = new StringBuilder(dna.Length);
            for (int i = dna.Length - 1; i >= 0; i--)
            {
                char c = dna[i];
                if (DnaComplement.TryGetValue(c, out var comp))
                {
                    sb.Append(comp);
                }
                else
                {
                    // Accept other letters but map to uppercase and keep
                    sb.Append(char.ToUpperInvariant(c));
                }
            }
            return sb.ToString();
        }

        public static string Transcribe(string? dna)
        {
            ArgumentNullException.ThrowIfNull(dna);
            if (dna.Length == 0) return string.Empty;
            // DNA -> RNA: replace T or t with U
            var sb = new StringBuilder(dna.Length);
            foreach (char c in dna)
            {
                if (c == 'T') sb.Append('U');
                else if (c == 't') sb.Append('u');
                else sb.Append(c);
            }
            return sb.ToString();
        }

        public static double GcContent(string? dna)
        {
            ArgumentNullException.ThrowIfNull(dna);
            if (dna.Length == 0) throw new ArgumentException("Sequence must not be empty.", nameof(dna));
            long gc = 0;
            long total = 0;
            foreach (char c in dna)
            {
                if (char.IsWhiteSpace(c)) continue;
                char cu = char.ToUpperInvariant(c);
                if (cu == 'G' || cu == 'C') gc++;
                if ("ATGCNatgcn".Contains(c)) total++;
                else throw new ArgumentException($"Invalid base '{c}' in sequence.");
            }
            return (double)gc / total;
        }

        public static IDictionary<string, int> KmerFrequencies(string? dna, int k)
        {
            ArgumentNullException.ThrowIfNull(dna);
            if (k <= 0) throw new ArgumentOutOfRangeException(nameof(k), "k must be > 0");
            var seq = dna.Trim();
            var dict = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i + k <= seq.Length; i++)
            {
                var sub = seq.Substring(i, k);
                dict.TryGetValue(sub, out var count);
                dict[sub] = count + 1;
            }
            return dict;
        }

        public static int HammingDistance(string? a, string? b)
        {
            ArgumentNullException.ThrowIfNull(a);
            ArgumentNullException.ThrowIfNull(b);
            if (a.Length != b.Length) throw new ArgumentException("Sequences must be the same length for Hamming distance.");
            int d = 0;
            for (int i = 0; i < a.Length; i++) if (a[i] != b[i]) d++;
            return d;
        }

        public static int LevenshteinDistance(string? a, string? b)
        {
            ArgumentNullException.ThrowIfNull(a);
            ArgumentNullException.ThrowIfNull(b);
            int n = a.Length, m = b.Length;
            if (n == 0) return m;
            if (m == 0) return n;
            var prev = new int[m + 1];
            var cur = new int[m + 1];
            for (int j = 0; j <= m; j++) prev[j] = j;
            for (int i = 1; i <= n; i++)
            {
                cur[0] = i;
                for (int j = 1; j <= m; j++)
                {
                    int cost = (a[i - 1] == b[j - 1]) ? 0 : 1;
                    cur[j] = Math.Min(Math.Min(prev[j] + 1, cur[j - 1] + 1), prev[j - 1] + cost);
                }
                Array.Copy(cur, prev, m + 1);
            }
            return prev[m];
        }
    }
}
