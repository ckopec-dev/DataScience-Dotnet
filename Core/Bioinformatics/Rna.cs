
using System.Text.RegularExpressions;

namespace Core.Bioinformatics
{
    public class Rna
    {
        private string _Code = String.Empty;

        public string Code
        {
            get { return _Code; }
            set
            {
                if (IsValidRna(value))
                    _Code = value;
                else
                    throw new ArgumentException("Invalid rna string.");
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
