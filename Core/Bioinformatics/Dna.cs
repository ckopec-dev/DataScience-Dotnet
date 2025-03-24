
using System.Text.RegularExpressions;

namespace Core.Bioinformatics
{
    public class Dna
    {
        protected string _Code = String.Empty;

        public string Code 
        { 
            get { return _Code.ToUpper(); }
            set 
            {
                if (IsValidDna(value))
                    _Code = value;
                else
                    throw new InvalidNucleotideException();
            }
        }

        public char[] Nucleotides
        {
            get { return _Code.ToCharArray(); }
        }

        public Dictionary<char, int> NucleotideCounts
        {
            get
            {
                Dictionary<char, int> dic = [];

                char[] chars = [.. _Code.Distinct()];

                foreach (char c in chars)
                {
                    dic.Add(c, _Code.Count(i => i == c));
                }

                return dic;
            }
        }

        public string ReverseCompliment
        {
            get
            {
                char[] reverse = Code.Reverse().ToCharArray();

                for(int i = 0; i < reverse.Length; i++)
                {
                    if (reverse[i] == 'A') reverse[i] = 'T'; 
                    else if (reverse[i] == 'T') reverse[i] = 'A'; 
                    else if (reverse[i] == 'C') reverse[i] = 'G';
                    else if (reverse[i] == 'G') reverse[i] = 'C';
                }

                return new string(reverse);
            }
        }

        public Dna()
        {
        }

        public Dna(string code)
        {
            Code = code;        
        }

        public override string ToString()
        {
            return Code;
        }

        public Rna ToRna()
        {
            return new Rna(Code.Replace("T", "U"));
        }

        public static bool IsValidDna(string code)
        {
            string pattern = "^[CAGT]+$";
            Regex rg = new(pattern);

            return rg.IsMatch(code);
        }

        public static int HammingDistance(Dna dna1, Dna dna2)
        {
            int d = 0;

            if (dna1.Nucleotides.Length != dna2.Nucleotides.Length)
                throw new InvalidComparisonException();

            for(int i = 0; i < dna1.Nucleotides.Length; i++)
            {
                if (dna1.Nucleotides[i] != dna2.Nucleotides[i])
                    d++;
            }

            return d;
        }
    }
}
