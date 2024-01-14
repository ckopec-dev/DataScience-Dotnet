
using Core;

namespace Rosalind
{
    public class BioinformaticsStronghold : ProblemDomain
    {
        #region Problems

        public static void ProblemDNA()
        {
            // https://rosalind.info/problems/dna/
            // Given: A DNA string s of length at most 1000 nt.
            // Return: Four integers(separated by spaces) counting the respective number of times that the symbols 'A', 'C', 'G', and 'T' occur in s.

            // Example input: AGCTTTTCATTCTGACTGCAACGGGCAATATGTCTCTGTGTGGATTAAAAAA AGAGTGTCTGATAGCAGC
            // Example output: 20 12 17 21

            string input = ReadInputToString();

            WriteOutput(String.Join(" ", input.AllIndexesOf("A").Count, input.AllIndexesOf("C").Count, input.AllIndexesOf("G").Count, input.AllIndexesOf("T").Count));
        }

        #endregion

        #region Helpers

        #endregion
    }
}
