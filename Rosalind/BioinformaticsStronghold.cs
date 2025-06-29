using Core;
using Core.Bioinformatics;
using System.Reflection;

namespace Rosalind
{
    public class BioinformaticsStronghold
    {
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
                Console.WriteLine("Month {0}: {1} adult pair(s), {2} pregnant pairs, {3} newborn pair(s), ({4} total pair(s)",
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
            Stream? mrs = Assembly.GetExecutingAssembly().GetManifestResourceStream("Rosalind.Inputs.gc.txt") ?? throw new Exception("Resource not found: gc.txt");
            using StreamReader sr = new(mrs);

            string input = sr.ReadToEnd().Trim();
            
            Fasta f = new(input);

            foreach (FastaEntry fe in f.Entries)
            {
                //Console.WriteLine("label: {0}", fe.Label);
                //Console.WriteLine("data: {0}", fe.Data);

                Dna dna = new(fe.Data);
                Console.WriteLine(dna.Code);
            }

            Dna d = new Dna("AGCTATAG");
            Console.WriteLine(d.GcContent);
            
            //throw new NotImplementedException();
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
            // see ProblemGC, which was accidentally skipped

            throw new NotImplementedException();
        }

        #endregion
    }
}
