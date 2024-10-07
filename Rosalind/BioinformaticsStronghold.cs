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

            Console.WriteLine(String.Join(" ", input.AllIndexesOf("A").Count, input.AllIndexesOf("C").Count, input.AllIndexesOf("G").Count, input.AllIndexesOf("T").Count));
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

        #endregion
    }
}
