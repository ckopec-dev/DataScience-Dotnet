using Core;
using Core.Bioinformatics;
using NLog;
using SkiaSharp;
using System.Numerics;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Rosalind
{
    public class BioinformaticsStronghold
    {
        private static readonly HttpClient client = new();

        #region Problems

        public static void ProblemDNA()
        {
            // https://rosalind.info/problems/dna/
            // Given: A DNA string s of length at most 1000 nt.
            // Return: Four integers(separated by spaces) counting the respective number of times that the symbols 'A', 'C', 'G', and 'T' occur in s.

            // Example input: AGCTTTTCATTCTGACTGCAACGGGCAATATGTCTCTGTGTGGATTAAAAAA AGAGTGTCTGATAGCAGC
            // Example output: 20 12 17 21

            Stream? mrs = Assembly.GetExecutingAssembly().GetManifestResourceStream("Rosalind.Inputs.dna.txt") ?? throw new Exception("Resource not found: dna.txt");
            using StreamReader sr = new(mrs);

            string input = sr.ReadToEnd().Trim();

            Dna dna = new(input);

            Console.WriteLine(String.Join(" ", dna.NucleotideCounts['A'], dna.NucleotideCounts['C'], dna.NucleotideCounts['G'], dna.NucleotideCounts['T']));
        }

        public static void ProblemRNA()
        {
            // https://rosalind.info/problems/rna/
            // Given: A DNA string t having length at most 1000 nt.
            // Return: The transcribed RNA string of t.

            // Example input: GATGGAACTTGACTACGTAAATT
            // Example output: GAUGGAACUUGACUACGUAAAUU

            Stream? mrs = Assembly.GetExecutingAssembly().GetManifestResourceStream("Rosalind.Inputs.rna.txt") ?? throw new Exception("Resource not found: rna.txt");
            using StreamReader sr = new(mrs);

            string input = sr.ReadToEnd().Trim();

            Dna dna = new(input);

            Console.WriteLine(dna.ToRna());
        }

        public static void ProblemREVC()
        {
            // https://rosalind.info/problems/revc/
            // Given: A DNA string s of length at most 1000 bp.
            // Return: The reverse complement of s.

            // Example input: AAAACCCGGT
            // Example output: ACCGGGTTTT

            Stream? mrs = Assembly.GetExecutingAssembly().GetManifestResourceStream("Rosalind.Inputs.revc.txt") ?? throw new Exception("Resource not found: revc.txt");
            using StreamReader sr = new(mrs);

            string input = sr.ReadToEnd().Trim();

            Dna dna = new(input);

            Console.WriteLine(dna.ReverseCompliment);
        }

        public static void ProblemFIB()
        {
            // Population rules:
            // The population begins in the first month with a pair of newborn rabbits.
            // Rabbits reach reproductive age after one month.
            
            // In any given month, every rabbit of reproductive age mates with another rabbit of reproductive age.
            // Exactly one month after two rabbits mate, they produce n pairs of one male and one female rabbit per pair.
            // Rabbits never die or stop reproducing.

            const long TOTAL_MONTHS = 31;
            const long LITTER_PAIRS = 5;

            long adult_pairs = 0;
            long pregnant_pairs = 0;
            long newborn_pairs = 1;

            for (long month = 1; month <= TOTAL_MONTHS; month++)
            {
                Console.WriteLine("Month {0}: {1} adult pair(s), {2} pregnant pairs, {3} newborn pair(s), {4} total pair(s)",
                    month, adult_pairs, pregnant_pairs, newborn_pairs, adult_pairs + newborn_pairs);

                #pragma warning disable IDE0059 // Unnecessary assignment of a value
                long current_adult_pairs = adult_pairs;
                long current_pregnant_pairs = pregnant_pairs;
                long current_newborn_pairs = newborn_pairs;
                #pragma warning restore IDE0059 // Unnecessary assignment of a value

                // All pregnant pairs produce LITTER_PAIRS newborns.
                newborn_pairs = current_pregnant_pairs * LITTER_PAIRS;

                // All newborns mature into adults.
                adult_pairs += current_newborn_pairs;

                // All adults mate.
                pregnant_pairs = adult_pairs;
            }
        }

        public static void ProblemGC()
        {
            Stream? mrs = Assembly.GetExecutingAssembly().GetManifestResourceStream("Rosalind.Inputs.gc.txt") ?? throw new ResourceNotFoundException();
            using StreamReader sr = new(mrs);

            string input = sr.ReadToEnd().Trim();
            
            Fasta f = new(input);

            decimal highest = 0;
            string? highest_label = null;

            foreach (FastaEntry fe in f.Entries)
            {
                Dna dna = new(fe.Data);

                Console.WriteLine("label: {0}", fe.Label);
                Console.WriteLine("data: {0}", fe.Data);
                Console.WriteLine("gc content: {0:0.0000}", dna.GcContent);

                decimal gc = dna.GcContent;
                if (gc > highest)
                {
                    highest = gc;
                    highest_label = fe.Label;
                }
            }

            Console.WriteLine("{0}\n\r{1}", highest_label, highest);
        }

        public static void ProblemHAMM()
        {
            Stream? mrs = Assembly.GetExecutingAssembly().GetManifestResourceStream("Rosalind.Inputs.hamm.txt") ?? throw new Exception("Resource not found: hamm.txt");
            using StreamReader sr = new(mrs);
            
            string? line1 = sr.ReadLine();
            string? line2 = sr.ReadLine();

            if (line1 == null || line2 == null) throw new Exception("Invalid resource.");
            
            Dna dna1 = new(line1);
            Dna dna2 = new(line2);

            Console.WriteLine("Hamming distance: {0}", Dna.HammingDistance(dna1, dna2));
        }

        public static void ProblemIPRB()
        {
            int k = 17; // = AA = 0
            int m = 16; // Aa = 1
            int n = 24; // aa = 2

            Random rnd = new();

            const int ttlIterations = 10000000;
            int ttlDominants = 0;

            for (int i = 0; i < ttlIterations; i++)
            {
                List<char> pop = [];

                for (int j = 0; j < k; j++)
                    pop.Add('k');
                for (int j = 0; j < m; j++)
                    pop.Add('m');
                for (int j = 0; j < n; j++)
                    pop.Add('n');

                // For this iteration, is org1 a k, m, or n?
                int idx = rnd.Next(pop.Count);
                char org1 = pop[idx];

                // What factor does org1 contribute?
                char factor1;
                if (org1 == 'k')
                    factor1 = 'A';
                else if (org1 == 'm')
                {
                    idx = rnd.Next(2);
                    if (idx == 0)
                        factor1 = 'A';
                    else
                        factor1 = 'a';
                }
                else
                    factor1 = 'a';

                // Find one instance of this organism(character) and remove it from the list.
                pop.Remove(org1);

                // Is org2 a k, m, or n?
                idx = rnd.Next(pop.Count);
                char org2 = pop[idx];

                // What factor does org2 contribute?
                char factor2;
                if (org2 == 'k')
                    factor2 = 'A';
                else if (org2 == 'm')
                {
                    idx = rnd.Next(2);
                    if (idx == 0)
                        factor2 = 'A';
                    else
                        factor2 = 'a';
                }
                else
                    factor2 = 'a';

                bool dominant = false;

                if (factor1 == 'A' || factor2 == 'A')
                    dominant = true;

                if (dominant)
                    ttlDominants++;
            }

            Console.WriteLine("{0} dominants out of {1} pairings = {2}.", ttlDominants, ttlIterations, ((decimal)ttlDominants / (decimal)ttlIterations));
        }

        public static void ProblemPROT()
        {
            Stream? mrs = Assembly.GetExecutingAssembly().GetManifestResourceStream("Rosalind.Inputs.prot.txt") ?? throw new ResourceNotFoundException();
            using StreamReader sr = new(mrs);

            Rna rna = new(sr.ReadToEnd());

            Console.WriteLine(rna.ToProteinString());
        }

        public static void ProblemSUBS()
        {
            Stream? mrs = Assembly.GetExecutingAssembly().GetManifestResourceStream("Rosalind.Inputs.subs.txt") ?? throw new ResourceNotFoundException();
            using StreamReader sr = new(mrs);

            string? s = sr.ReadLine() ?? throw new ResourceNotFoundException();
            string? t = sr.ReadLine() ?? throw new ResourceNotFoundException();

            List<int> indexes = s.AllIndexesOf(t, 1);

            Console.WriteLine(indexes.PrettyPrint().Replace(",", ""));
        }

        public static void ProblemCONS()
        {
            // see ProblemGC

            Stream? mrs = Assembly.GetExecutingAssembly().GetManifestResourceStream("Rosalind.Inputs.cons.txt") ?? throw new ResourceNotFoundException();
            using StreamReader sr = new(mrs);

            string input = sr.ReadToEnd().Trim();

            Fasta f = new(input);

            List<Dna> dnaStrings = [];
            foreach (FastaEntry fe in f.Entries)
                dnaStrings.Add(new Dna(fe.Data));

            ProfileMatrix pm = new(dnaStrings);
            Console.WriteLine(pm.Consensus);
            Console.WriteLine(pm);
        }

        public static void ProblemFIBD()
        {
            // Given: Positive integers n ≤ 100 and m ≤ 20.
            // Return: The total number of pairs of rabbits that will remain after the n-th month if all rabbits live for m months. 1 pair per litter.

            int totalMonths = 92;
            int lifespan = 20;

            BigInteger[] newRabbits = new BigInteger[totalMonths];
            BigInteger[] matureRabbits = new BigInteger[totalMonths];

            newRabbits[0] = 1;
            newRabbits[1] = 0;
            matureRabbits[0] = 0;
            matureRabbits[1] = 1;

            for (int i = 2; i < totalMonths; i++)
            {
                matureRabbits[i] = matureRabbits[i - 1] + newRabbits[i - 1];
                newRabbits[i] = matureRabbits[i - 1];

                // Subtract the number that died
                if (i >= lifespan)
                {
                    matureRabbits[i] = matureRabbits[i] - newRabbits[i - lifespan];
                }
            }

            BigInteger totalRabbits = matureRabbits[totalMonths - 1] + newRabbits[totalMonths - 1];

            Console.WriteLine(totalRabbits);
        }

        public static void ProblemGRPH()
        {
            Stream? mrs = Assembly.GetExecutingAssembly().GetManifestResourceStream("Rosalind.Inputs.grph.txt") ?? throw new ResourceNotFoundException();
            using StreamReader sr = new(mrs);

            int k = 3; // Rosalind GRPH uses k = 3
            
            var records = ReadFastaGRPH(sr);

            // Build a map: prefix(k) -> list of record IDs with that prefix
            var prefixIndex = new Dictionary<string, List<string>>(StringComparer.Ordinal);
            foreach (var r in records)
            {
                if (r.Seq.Length < k) continue;
                var pre = r.Seq[..k];
                if (!prefixIndex.TryGetValue(pre, out var list))
                    prefixIndex[pre] = list = [];
                list.Add(r.Id);
            }

            // For each record, find others whose prefix matches this record's suffix
            foreach (var r in records)
            {
                if (r.Seq.Length < k) continue;
                var suf = r.Seq.Substring(r.Seq.Length - k, k);
                if (prefixIndex.TryGetValue(suf, out var candidates))
                {
                    foreach (var toId in candidates)
                    {
                        if (!ReferenceEquals(r.Id, toId) && r.Id != toId) // avoid self-loops
                            Console.WriteLine($"{r.Id} {toId}");
                    }
                }
            }
        }

        public static void ProblemIEV()
        {
            Stream? mrs = Assembly.GetExecutingAssembly().GetManifestResourceStream("Rosalind.Inputs.iev.txt") ?? throw new ResourceNotFoundException();
            using StreamReader sr = new(mrs);

            string input = sr.ReadToEnd().Trim();

            double result = CalculateExpectedOffspring_IEV(input);
            Console.WriteLine(result.ToString("F1")); // Format to 1 decimal place
        }

        public static void ProblemLCSM()
        {
            Stream? mrs = Assembly.GetExecutingAssembly().GetManifestResourceStream("Rosalind.Inputs.lcsm.txt") ?? throw new ResourceNotFoundException();
            using StreamReader sr = new(mrs);

            List<string> lines = [];

            string input = sr.ReadToEnd().Trim();

            Fasta f = new(input);

            foreach(var fe in f.Entries)
            {
                lines.Add(fe.Data);
            }
            
            string? result = lines.LargestCommonSubstring();
            Console.WriteLine(result); 
        }

        public static void ProblemLIA()
        {
            Stream? mrs = Assembly.GetExecutingAssembly().GetManifestResourceStream("Rosalind.Inputs.lia.txt") ?? throw new ResourceNotFoundException();
            using StreamReader sr = new(mrs);

            // Read input: k (generation) and N (minimum number of AaBb offspring)
            string[] input = sr!.ReadLine()!.Split();
            int k = int.Parse(input[0]);
            int N = int.Parse(input[1]);

            // Calculate total offspring in generation k
            int totalOffspring = (int)Math.Pow(2, k);

            // Probability that any single offspring is AaBb
            // When both parents are AaBb, probability of AaBb offspring is 1/4
            double probAaBb = 0.25;

            // Calculate probability that at least N offspring are AaBb
            // This is 1 - P(fewer than N are AaBb)
            double result = 1.0 - CumulativeBinomial_LIA(totalOffspring, probAaBb, N - 1);

            Console.WriteLine($"{result:F3}");
        }

        public static void ProblemMPRT()
        {
            Console.WriteLine("Web site has changed, problem no longer solvable as designed.");
        }

        #endregion

        #region Helpers

        // Calculate cumulative binomial probability P(X <= k)
        static double CumulativeBinomial_LIA(int n, double p, int k)
        {
            double sum = 0.0;
            for (int i = 0; i <= k; i++)
            {
                sum += BinomialProbability_LIA(n, p, i);
            }
            return sum;
        }

        // Calculate binomial probability P(X = k) = C(n,k) * p^k * (1-p)^(n-k)
        static double BinomialProbability_LIA(int n, double p, int k)
        {
            if (k > n || k < 0) return 0.0;

            double coefficient = BinomialCoefficient_LIA(n, k);
            double probability = Math.Pow(p, k) * Math.Pow(1 - p, n - k);

            return coefficient * probability;
        }

        // Calculate binomial coefficient C(n, k) = n! / (k! * (n-k)!)
        static double BinomialCoefficient_LIA(int n, int k)
        {
            if (k > n - k) k = n - k; // Take advantage of symmetry

            double result = 1.0;
            for (int i = 0; i < k; i++)
            {
                result = result * (n - i) / (i + 1);
            }

            return result;
        }

        class FastaRecordGRPH
        {
            public string Id { get; set; } = "";
            public string Seq { get; set; } = "";
        }

        static List<FastaRecordGRPH> ReadFastaGRPH(TextReader reader)
        {
            var result = new List<FastaRecordGRPH>();
            FastaRecordGRPH? current = null;
            string? line;

            while ((line = reader.ReadLine()) != null)
            {
                if (line.Length == 0) continue;

                if (line[0] == '>')
                {
                    // Start a new record. Use the first token after '>' as the ID (handles descriptions).
                    var header = line[1..].Trim();
                    #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                    var id = header.Split((char[])null, StringSplitOptions.RemoveEmptyEntries)[0];
                    #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                    current = new FastaRecordGRPH { Id = id, Seq = "" };
                    result.Add(current);
                }
                else
                {
                    if (current == null)
                        throw new InvalidDataException("FASTA format error: sequence data before any header.");
                    current.Seq += line.Trim();
                }
            }
            return result;
        }

        /// <summary>
        /// Alternative method with explicit genotype combinations for clarity
        /// </summary>
        static double CalculateExpectedOffspringDetailed_IEV(string input)
        {
            int[] couples = [.. input.Split(' ').Select(int.Parse)];

            // Genotype combinations and their probabilities of producing dominant offspring
            var genotypeData = new[]
            {
                new { Name = "AA-AA", Couples = couples[0], DominantProb = 1.0 },
                new { Name = "AA-Aa", Couples = couples[1], DominantProb = 1.0 },
                new { Name = "AA-aa", Couples = couples[2], DominantProb = 1.0 },
                new { Name = "Aa-Aa", Couples = couples[3], DominantProb = 0.75 },
                new { Name = "Aa-aa", Couples = couples[4], DominantProb = 0.5 },
                new { Name = "aa-aa", Couples = couples[5], DominantProb = 0.0 }
            };

            double totalExpected = 0;

            foreach (var genotype in genotypeData)
            {
                double expectedFromThisGenotype = genotype.Couples * 2 * genotype.DominantProb;
                totalExpected += expectedFromThisGenotype;

                // Uncomment for debugging:
                // Console.WriteLine($"{genotype.Name}: {genotype.Couples} couples * 2 offspring * {genotype.DominantProb} prob = {expectedFromThisGenotype}");
            }

            return totalExpected;
        }

        /// <summary>
        /// Calculates the expected number of offspring displaying the dominant phenotype
        /// </summary>
        /// <param name="input">String containing 6 integers representing couples of each genotype pair</param>
        /// <returns>Expected number of dominant offspring</returns>
        static double CalculateExpectedOffspring_IEV(string input)
        {
            // Parse the input to get the number of couples for each genotype combination
            int[] couples = [.. input.Split(' ').Select(int.Parse)];

            // Expected offspring per couple = 2 (each couple produces 2 offspring on average)
            int offspringPerCouple = 2;

            // Probability of dominant phenotype for each genotype combination:
            // AA-AA: 1.0 (all offspring will be AA - dominant)
            // AA-Aa: 1.0 (all offspring will be either AA or Aa - dominant)
            // AA-aa: 1.0 (all offspring will be Aa - dominant)
            // Aa-Aa: 0.75 (AA: 0.25, Aa: 0.5, aa: 0.25 -> 0.75 dominant)
            // Aa-aa: 0.5 (Aa: 0.5, aa: 0.5 -> 0.5 dominant)
            // aa-aa: 0.0 (all offspring will be aa - recessive)

            double[] dominantProbabilities = [1.0, 1.0, 1.0, 0.75, 0.5, 0.0];

            double expectedDominantOffspring = 0;

            // Calculate expected dominant offspring for each genotype combination
            for (int i = 0; i < couples.Length; i++)
            {
                expectedDominantOffspring += couples[i] * offspringPerCouple * dominantProbabilities[i];
            }

            return expectedDominantOffspring;
        }
    }

    #endregion
}

