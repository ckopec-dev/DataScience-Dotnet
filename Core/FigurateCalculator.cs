
namespace Core
{
    public static class FigurateCalculator
    {
        public static FigurateNumber Triangle(int n)
        {
            return new FigurateNumber(n * (n + 1) / 2, FigurateType.Triangle);
        }

        public static FigurateNumber Square(int n)
        {
            return new FigurateNumber(n * n, FigurateType.Square);
        }

        public static FigurateNumber Pentagonal(int n)
        {
            return new FigurateNumber(n * (3 * n - 1) / 2, FigurateType.Pentagonal);
        }

        public static FigurateNumber Hexagonal(int n)
        {
            return new FigurateNumber(n * (2 * n - 1), FigurateType.Hexagonal);
        }

        public static FigurateNumber Heptagonal(int n)
        {
            return new FigurateNumber(n * (5 * n - 3) / 2, FigurateType.Heptagonal);
        }

        public static FigurateNumber Octagonal(int n)
        {
            return new FigurateNumber(n * (3 * n - 2), FigurateType.Octagonal);
        }
    }
}
