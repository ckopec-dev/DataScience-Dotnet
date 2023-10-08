
namespace Core
{
    public static class MathHelper
    {
        public static bool IsPrime(long n)
        {
            if (n < 2) return false;
            if (n == 2) return true;
            if (n % 2 == 0) return false;

            for (int i = 3; i * i <= n; i += 2)
            {
                if (n % i == 0)
                    return false;
            }

            return true;
        }

        public static List<long> ProperDivisors(long n)
        {
            List<long> d = new();

            if (n == 0)
                return d;

            d.Add(1);

            if (n < 2)
            {
                return d;
            }
            else
            {
                for (int i = 2; i < n; i++)
                {
                    if (n % i == 0)
                        d.Add(i);
                }
            }

            return d;
        }
    }
}
