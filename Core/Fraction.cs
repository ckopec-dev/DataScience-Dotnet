
using System.Runtime.InteropServices.Marshalling;

namespace Core
{
    public class Fraction
    {
        public long Numerator { get; set; }
        public long Denominator { get; set; }
        public double Value
        {
            get { return (double)Numerator / (double)Denominator; }
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
    }
}
