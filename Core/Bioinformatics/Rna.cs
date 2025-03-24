
using System.Text.RegularExpressions;

namespace Core.Bioinformatics
{
    public class Rna
    {
        protected string _Code = String.Empty;

        public string Code
        {
            get { return _Code.ToUpper(); }
            set
            {
                if (IsValidRna(value))
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

        public Rna()
        {
        }

        public Rna(string code)
        {
            Code = code;
        }

        public override string ToString()
        {
            return Code;
        }

        public static bool IsValidRna(string code)
        {
            string pattern = "^[CAGU]+$";
            Regex rg = new(pattern);

            return rg.IsMatch(code);
        }
    }
}
