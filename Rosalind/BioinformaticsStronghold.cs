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

            throw new NotImplementedException();
        }

        public static void ProblemHAMM()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
