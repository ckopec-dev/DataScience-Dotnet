
using Core;

namespace Rosalind
{
    public class BioinformaticsArmory : ProblemDomain
    {
        #region Problems

        public static void ProblemINI()
        {
            // https://rosalind.info/problems/ini/
            // Given: A DNA string s of length at most 1000 bp.
            // Return: Four integers(separated by spaces) representing the respective number of times that the symbols 'A', 'C', 'G', and 'T' occur in s.
            // Note: You must provide your answer in the format shown in the sample output below.

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
