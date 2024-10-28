
namespace Core
{
    public class FigurateNumber
    {
        public int Number;
        public FigurateType Type;

        public FigurateNumber()
        {
        }

        public FigurateNumber(int number, FigurateType type)
        {
            Number = number;
            Type = type;
        }

        public static List<int> ToList(List<FigurateNumber> figurateNumbers)
        {
            List<int> ints = [];

            foreach(FigurateNumber fn in figurateNumbers)
            {
                ints.Add(fn.Number);
            }

            return ints;
        }
    }
}
