
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

        public Dictionary<char, int> NucleotideCounts
        {
            get
            {
                Dictionary<char, int> dic = [];
                
                char[] chars = _Code.Distinct().ToArray();

                foreach (char c in chars)
                {
                    dic.Add(c, _Code.Count(i => i == c));
                }

                return dic;
            }
        }
    }
}
