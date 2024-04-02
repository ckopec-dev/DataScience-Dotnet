
using System.Numerics;

namespace Core
{
    public static class Statistics
    {
        public static decimal Mean(decimal[] values)
        {
            return values.Sum() / values.Length;
        }

        public static decimal Average(decimal[] values)
        {
            return Mean(values);
        }

        public static decimal Median(decimal[] values)
        {
            Array.Sort(values);
            return values[values.Length / 2];
        }

        public static int Mode(int[] values)
        {
            return values.GroupBy(v => v)
            .OrderByDescending(g => g.Count())
            .First()
            .Key;
        }

        public static decimal Range(decimal[] values)
        {
            return values.Max() - values.Min();
        }

        public static decimal Variance(decimal[] values)
        {
            if (values.Length > 1)
            {
                decimal avg = Average(values);
                decimal variance = 0;
                foreach (decimal value in values)
                {
                    variance += (decimal)Math.Pow((double)(value - avg), 2.0);
                }
                return variance / values.Length;
            }
            else
            {
                return 0;
            }
        }

        public static decimal StdDeviation(decimal[] values) 
        {
            return (decimal)Math.Sqrt((double)Variance(values));
        }

        public static BigInteger BinomialCoefficient(long items, long size)
        {
            // https://en.wikipedia.org/wiki/Binomial_coefficient

            if (size > items)
                return BigInteger.Zero;

            BigInteger result = BigInteger.One;

            for(BigInteger i = 1; i <= size; i++)
            {
                result *= items--;
                result /= i;
            }

            return result;
        }
    }
}
