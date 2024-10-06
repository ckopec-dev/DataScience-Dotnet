
using System.Text.RegularExpressions;

namespace Core.Bioinformatics
{
    public class Dna
    {
        private string _Code = String.Empty;
        
        public string Code 
        { 
            get { return _Code; }
            set 
            {
                if (IsValidDna(value))
                    _Code = value;
                else
                    throw new ArgumentException("Invalid dna string.");
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

        public static bool IsValidDna(string code)
        {
            string pattern = "^[CAGT]+$";
            Regex rg = new(pattern);

            return rg.IsMatch(code);
        }
    }
}
