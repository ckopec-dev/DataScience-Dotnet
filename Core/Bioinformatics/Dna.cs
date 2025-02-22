
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
                    throw new InvalidNucleotideException();
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
                        _ => throw new InvalidNucleotideException()
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
