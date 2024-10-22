
using System.Text.RegularExpressions;

namespace Core.Bioinformatics
{
    public class Dna : NucleicAcid
    {
        public override string Code 
        { 
            get { return _Code.ToUpper(); }
            set 
            {
                if (IsValidDna(value))
                    _Code = value;
                else
                    throw new ArgumentException("Invalid dna string.");
            }
        }

        public string ReverseCompliment
        {
            get
            {
                char[] reverse = Code.Reverse().ToCharArray();

                for(int i = 0; i < reverse.Length; i++)
                {
                    reverse[i] = reverse[i] switch
                    {
                        'A' => 'T',
                        'T' => 'A',
                        'C' => 'G',
                        'G' => 'C',
                        _ => throw new Exception(String.Format("Invalid nucleotide found: '{0}'.", reverse[i])),
                    };
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
    }
}
