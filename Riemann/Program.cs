using System.Numerics;

namespace Riemann
{
    internal class Program
    {
        static void Main()
        {
            Zeta(new Complex(-0.5d, 0));
        }

        public static Complex Zeta(Complex s)
        {
            // 1 + 1/2^s + 1/3^s ...

            const double terms = 10d;

            Complex c = new Complex(-1d, 8d);

            for(double i = 2; i < terms; i++)
            {
                c += (1 / Complex.Pow(new Complex(i, 0), s));

                Console.WriteLine("c: {0}", c);
            }

            return c;
        }
    }
}