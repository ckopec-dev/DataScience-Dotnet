
namespace Core.Bioinformatics
{
    public class NucleicAcid
    {
        protected string _Code = String.Empty;

        public virtual string Code
        {
            get { return _Code; }
            set { _Code = value; }
        }

        public char[] Nucleotides
        {
            get { return Code.ToCharArray(); }
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
    }
}
