
using System.Text;

namespace Core.Bioinformatics
{
    public class ProfileMatrix
    {
        public List<int> A { get; set; } = [];
        public List<int> C { get; set; } = [];
        public List<int> G { get; set; } = [];
        public List<int> T { get; set; } = [];

        public string Consensus
        {
            get
            {
                // The consensus is the base that occurs with the most frequency.

                StringBuilder sb = new();

                for (int i = 0; i < A.Count; i++)
                {
                    char val = 'A';
                    int max = A[i];

                    if (C[i] > max)
                    {
                        val = 'C';
                        max = C[i];
                    }
                    if (G[i] > max)
                    {
                        val = 'G';
                        max = G[i];
                    }
                    if (T[i] > max)
                    {
                        val = 'T';
                    }

                    sb.Append(val);
                }

                return sb.ToString();
            }
        }

        public ProfileMatrix(List<Dna> dnaStrings)
        {
            // All dna strings must be of equal length.
            for(int i = 1; i < dnaStrings.Count; i++)
            {
                if (dnaStrings[i].Code.Length != dnaStrings[0].Code.Length)
                    throw new InvalidComparisonException();
            }

            for (int i = 0; i < dnaStrings[0].Code.Length; i++)
            {
                A.Add(0);
                C.Add(0);
                G.Add(0);
                T.Add(0);

                foreach (Dna dna in dnaStrings)
                {
                    string j = dna.Code.Substring(i, 1);

                    if (j == "A") A[i]++;
                    if (j == "C") C[i]++;
                    if (j == "G") G[i]++;
                    if (j == "T") T[i]++;
                }
            }
        }

        public override string ToString()
        {
            // Example output:

            // A: 5 1 0 0 5 5 0 0
            // C: 0 0 1 4 2 0 6 1
            // G: 1 1 6 3 0 1 0 0
            // T: 1 5 0 0 0 1 1 6

            StringBuilder sb = new();

            sb.AppendLine("A: " + String.Join(" ", A));
            sb.AppendLine("C: " + String.Join(" ", C));
            sb.AppendLine("G: " + String.Join(" ", G));
            sb.AppendLine("T: " + String.Join(" ", T));

            return sb.ToString();
        }
    }
}
