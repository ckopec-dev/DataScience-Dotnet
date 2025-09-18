namespace Core.Maths
{
    public class Fraction
    {
        public long Numerator { get; set; }
        public long Denominator { get; set; }
        public double Value
        {
            get { return Numerator / (double)Denominator; }
        }

        public Fraction()
        {
        }

        public Fraction(int n, int d)
        {
            Numerator = n;
            Denominator = d;
        }

        public Fraction(uint n, uint d)
        {
            Numerator = n;
            Denominator = d;
        }

        public Fraction(long n, long d)
        {
            Numerator = n;
            Denominator = d;
        }

        public Fraction(ulong n, ulong d)
        {
            Numerator = (long)n;
            Denominator = (long)d;
        }

        public static Fraction Add(Fraction a, Fraction b)
        {
            long denominator = MathHelper.GCD(a.Denominator, b.Denominator);

            denominator = a.Denominator * b.Denominator / denominator;
            long numerator = a.Numerator * (denominator / a.Denominator) +
                b.Numerator * (denominator / b.Denominator);

            long common_factor = MathHelper.GCD(numerator, denominator);

            denominator /= common_factor;
            numerator /= common_factor;

            return new Fraction(numerator, denominator);
        }

        public override string ToString()
        {
            return string.Format("{0} / {1}", Numerator, Denominator);
        }
    }
}
