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

            const int TOTAL_MONTHS = 5;
            const int LITTER_PAIRS = 1;

            int adult_pairs = 0;
            int newborn_pairs = 1;

            for (int month = 1; month <= TOTAL_MONTHS; month++)
            {
                Console.WriteLine("Month {0}: {1} adult pair(s), {2} newborn pair(s), ({3} total pair(s)",
                    month, adult_pairs, newborn_pairs, adult_pairs + newborn_pairs);

                // All existing newborns turn into adults.
                adult_pairs += newborn_pairs;
                newborn_pairs = 0;
            }

            //throw new NotImplementedException();
        }

        #endregion
    }
}
