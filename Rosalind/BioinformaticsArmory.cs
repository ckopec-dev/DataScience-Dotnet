
using Core;
using System.Reflection;

namespace Rosalind
{
    public class BioinformaticsArmory
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

            Stream? mrs = Assembly.GetExecutingAssembly().GetManifestResourceStream("Rosalind.Inputs.ini.txt") ?? throw new Exception("Resource not found: ini.txt");
            using StreamReader sr = new(mrs);

            string input = sr.ReadToEnd();

            Console.WriteLine(String.Join(" ", input.AllIndexesOf("A").Count, input.AllIndexesOf("C").Count, input.AllIndexesOf("G").Count, input.AllIndexesOf("T").Count));
        }

        public static void ProblemGBK()
        {
            // GenBank web site has changed and this problem no longer works with it. Skipping.
        }

        #endregion

        #region Helpers

        #endregion
    }
}
