using System.Numerics;

namespace Collatz
{
    /// <summary>
    /// The conjecture: https://en.wikipedia.org/wiki/Collatz_conjecture
    /// For plotting from a console app: https://scottplot.net/quickstart/console/
    /// </summary>
    internal class Program
    {
        static readonly Random rnd = new();

        static void Main(string[] args)
        {
            string switchErr = "Switch missing or invalid.";

            if (args != null && args.Length == 1)
            {
                switch (args[0])
                {
                    case "/randombiginttest": RandomBigIntTest(); break;
                    case "/randomlargesearch": RandomLargeSearch(); break;
                    case "/averagestoppingtime": AverageStoppingTime(); break;
                    case "/calculatetest": CalculateTest(); break;
                    case "/iteratetest": IterateTest(); break;
                    case "/plottest": PlotTest(); break;
                    case "/stoppingtimescatterplot": StoppingTimeScatterPlot(9999); break;
                    case "/stoppingtimehistogram": StoppingTimeHistogram(1000, 2, 3, 1); break;
                    default: Console.WriteLine(switchErr); break;
                }
            }
            else
            {
                Console.WriteLine(switchErr);
            }
        }

        #region Experiments/Tests 

        static void RandomBigIntTest()
        {
            for (int i = 1; i <= 20; i++)
            {
                Console.WriteLine("{0}: {1}", i, RandomBigInt(i));
            }
        }

        static void RandomLargeSearch()
        {
            for (int i = 0; i < 10; i++)
            {
                BigInteger n = RandomBigInt(100000);

                BigInteger st = BigIterate(n);

                Console.WriteLine("{0}", st);
            }
        }

        static void AverageStoppingTime()
        {
            const int iterations = 100;
            const int upperBound = 100;

            double[] dataX = new double[upperBound];
            double[] dataY = new double[upperBound];

            for (int digits = 1; digits <= upperBound; digits++)
            {
                Console.WriteLine("Average stopping time for {0} digit(s).", digits);
                dataX[digits - 1] = digits;

                BigInteger stSum = new();

                for (int i = 0; i < iterations; i++)
                {
                    BigInteger n = RandomBigInt(digits);

                    BigInteger st = BigIterate(n);

                    stSum += st;
                }

                dataY[digits - 1] = (double)(stSum / iterations);
            }

            var plt = new ScottPlot.Plot(1200, 800);
            plt.AddBar(dataY);
            new ScottPlot.FormsPlotViewer(plt).ShowDialog();
        }

        static void CalculateTest()
        {
            Console.WriteLine("5: {0}", Calculate(5));
        }

        static void IterateTest()
        {
            // This test results in an infinite loop...
            Console.WriteLine("Steps: {0}", Iterate(5, 2, 5, 1));
        }

        static void PlotTest()
        {
            double[] dataX = new double[] { 1, 2, 3, 4, 5 };
            double[] dataY = new double[] { 1, 4, 9, 16, 25 };
            var plt = new ScottPlot.Plot(400, 300);
            plt.AddScatter(dataX, dataY);
            new ScottPlot.FormsPlotViewer(plt).ShowDialog();
        }

        static void StoppingTimeScatterPlot(int max)
        {
            // Numbers from 1 to 9999 and their corresponding total stopping time.

            double[] dataX = new double[max];
            double[] dataY = new double[max];

            for (long n = 1; n <= max; n++)
            {
                dataX[n - 1] = n;
                dataY[n - 1] = Iterate(n, 2, 3, 1);
            }

            var plt = new ScottPlot.Plot(1200, 800);
            plt.AddScatter(dataX, dataY, null, 1, 5, ScottPlot.MarkerShape.filledCircle, ScottPlot.LineStyle.None, null);
            new ScottPlot.FormsPlotViewer(plt).ShowDialog();
        }

        static void StoppingTimeHistogram(int max, long evenDivisor, long oddMultiplier, long oddAddition)
        {
            // Histogram of total stopping times for the numbers 1 to 10^8.
            // Total stopping time is on the x axis, frequency on the y axis.

            // Ultimately we want 2 double arrays, similar to scatter plot demo. 
            // But we won't know how big the array is until we're done with all the calculations.

            // Store calculations in a dictionary and then convert to dual arrays when done.

            Dictionary<long, long> buckets = new();

            for (long n = 1; n <= max; n++)
            {
                if (n % 1000000 == 0)
                    Console.WriteLine("Calculating n = {0}.", n);

                long stoppingTime = Iterate(n, evenDivisor, oddMultiplier, oddAddition);

                if (!buckets.ContainsKey(stoppingTime))
                    buckets.Add(stoppingTime, 0);

                buckets[stoppingTime] += 1;
            }

            Console.WriteLine("Key count: {0}", buckets.Count);

            long largestKey = buckets.Keys.Max();

            Console.WriteLine("Largest key: {0}", largestKey);

            double[] dataX = new double[largestKey + 1];
            double[] dataY = new double[largestKey + 1];

            for (long x = 0; x < dataX.Length; x++)
            {
                dataX[x] = x;
            }

            foreach (var item in buckets)
            {
                dataY[item.Key] = item.Value;
            }

            var plt = new ScottPlot.Plot(1200, 800);
            plt.AddBar(dataY);
            new ScottPlot.FormsPlotViewer(plt).ShowDialog();
        }

        #endregion

        #region Helpers

        static BigInteger RandomBigInt(int digits)
        {
            if (digits < 1) throw new Exception("Invalid input.");

            string s = "";

            for (int i = 0; i < digits; i++)
            {
                int val = rnd.Next(0, 10);

                // For numbers with more than one digit, make sure the first digit is not zero.
                while (digits > 1 && i == 0 && val == 0)
                    val = rnd.Next(0, 10);

                s += val.ToString();
            }

            return BigInteger.Parse(s);
        }

        static BigInteger BigIterate(BigInteger n)
        {
            int step = 0;

            do
            {
                step++;

                n = BigCalculate(n);
            }
            while (n > 1);

            return step;
        }

        static BigInteger BigCalculate(BigInteger n)
        {
            if (n < 1)
                return BigInteger.Zero;

            if (n % 2 == 0) return n / 2;
            else return (n * 3) + 1;
        }

        static long Calculate(long n)
        {
            return Calculate(n, 2, 3, 1);
        }

        static long Calculate(long n, long evenDivisor, long oddMultiplier, long oddAddition)
        {
            // If the number is even, divide it by evenDivisor.
            // If the number is odd, multiply by oddMultipler and add oddAddition.

            if (n % 2 == 0) return n / evenDivisor;
            else return (n * oddMultiplier) + oddAddition;
        }

        static long Iterate(long n, long evenDivisor, long oddMultiplier, long oddAddition)
        {
            if (n < 1)
                throw new Exception("Invalid input.");

            int step = 0;

            do
            {
                step++;

                n = Calculate(n, evenDivisor, oddMultiplier, oddAddition);

                Console.WriteLine(n);
            }
            while (n != 1);

            return step;
        }

        #endregion
    }
}