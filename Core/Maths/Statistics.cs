namespace Core.Maths
{
    /// <summary>
    /// Comprehensive statistics class providing descriptive statistics, probability distributions,
    /// hypothesis testing, and correlation analysis functionality.
    /// </summary>
    public static class Statistics
    {
        private static readonly double SQRT_2PI = Math.Sqrt(2 * Math.PI);
        private static readonly double LOG_SQRT_2PI = Math.Log(SQRT_2PI);

        #region Descriptive Statistics

        /// <summary>
        /// Calculates the arithmetic mean of a dataset.
        /// </summary>
        public static double Mean(IEnumerable<double> values)
        {
            ArgumentNullException.ThrowIfNull(values);
            var data = values.ToList();
            if (data.Count == 0) throw new ArgumentException("Dataset cannot be empty");
            return data.Sum() / data.Count;
        }

        /// <summary>
        /// Calculates the median of a dataset.
        /// </summary>
        public static double Median(IEnumerable<double> values)
        {
            ArgumentNullException.ThrowIfNull(values);
            var sorted = values.OrderBy(x => x).ToList();
            if (sorted.Count == 0) throw new ArgumentException("Dataset cannot be empty");

            int n = sorted.Count;
            if (n % 2 == 0)
                return (sorted[n / 2 - 1] + sorted[n / 2]) / 2.0;
            return sorted[n / 2];
        }

        /// <summary>
        /// Calculates the mode(s) of a dataset.
        /// </summary>
        public static IEnumerable<double> Mode(IEnumerable<double> values)
        {
            ArgumentNullException.ThrowIfNull(values);
            var data = values.ToList();
            if (data.Count == 0) throw new ArgumentException("Dataset cannot be empty");

            var frequency = data.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());
            int maxFreq = frequency.Values.Max();
            return frequency.Where(kv => kv.Value == maxFreq).Select(kv => kv.Key);
        }

        /// <summary>
        /// Calculates the population variance of a dataset.
        /// </summary>
        public static double PopulationVariance(IEnumerable<double> values)
        {
            ArgumentNullException.ThrowIfNull(values);
            var data = values.ToList();
            if (data.Count == 0) throw new ArgumentException("Dataset cannot be empty");

            double mean = Mean(data);
            return data.Sum(x => Math.Pow(x - mean, 2)) / data.Count;
        }

        /// <summary>
        /// Calculates the sample variance of a dataset.
        /// </summary>
        public static double SampleVariance(IEnumerable<double> values)
        {
            ArgumentNullException.ThrowIfNull(values);
            var data = values.ToList();
            if (data.Count < 2) throw new ArgumentException("Sample variance requires at least 2 values");

            double mean = Mean(data);
            return data.Sum(x => Math.Pow(x - mean, 2)) / (data.Count - 1);
        }

        /// <summary>
        /// Calculates the population standard deviation of a dataset.
        /// </summary>
        public static double PopulationStandardDeviation(IEnumerable<double> values)
        {
            return Math.Sqrt(PopulationVariance(values));
        }

        /// <summary>
        /// Calculates the sample standard deviation of a dataset.
        /// </summary>
        public static double SampleStandardDeviation(IEnumerable<double> values)
        {
            return Math.Sqrt(SampleVariance(values));
        }

        /// <summary>
        /// Calculates the range of a dataset.
        /// </summary>
        public static double Range(IEnumerable<double> values)
        {
            ArgumentNullException.ThrowIfNull(values);
            var data = values.ToList();
            if (data.Count == 0) throw new ArgumentException("Dataset cannot be empty");
            return data.Max() - data.Min();
        }

        /// <summary>
        /// Calculates the interquartile range (IQR) of a dataset.
        /// </summary>
        public static double InterquartileRange(IEnumerable<double> values)
        {
            return Percentile(values, 75) - Percentile(values, 25);
        }

        /// <summary>
        /// Calculates the specified percentile of a dataset.
        /// </summary>
        public static double Percentile(IEnumerable<double> values, double percentile)
        {
            ArgumentNullException.ThrowIfNull(values);
            if (percentile < 0 || percentile > 100) throw new ArgumentException("Percentile must be between 0 and 100");

            var sorted = values.OrderBy(x => x).ToList();
            if (sorted.Count == 0) throw new ArgumentException("Dataset cannot be empty");

            if (percentile == 0) return sorted.First();
            if (percentile == 100) return sorted.Last();

            double index = (percentile / 100.0) * (sorted.Count - 1);
            int lowerIndex = (int)Math.Floor(index);
            int upperIndex = (int)Math.Ceiling(index);

            if (lowerIndex == upperIndex)
                return sorted[lowerIndex];

            double weight = index - lowerIndex;
            return sorted[lowerIndex] * (1 - weight) + sorted[upperIndex] * weight;
        }

        /// <summary>
        /// Calculates the skewness of a dataset.
        /// </summary>
        public static double Skewness(IEnumerable<double> values)
        {
            ArgumentNullException.ThrowIfNull(values);
            var data = values.ToList();
            if (data.Count < 3) throw new ArgumentException("Skewness requires at least 3 values");

            double mean = Mean(data);
            double stdDev = SampleStandardDeviation(data);
            double n = data.Count;

            double skew = data.Sum(x => Math.Pow((x - mean) / stdDev, 3)) * n / ((n - 1) * (n - 2));
            return skew;
        }

        /// <summary>
        /// Calculates the kurtosis of a dataset.
        /// </summary>
        public static double Kurtosis(IEnumerable<double> values)
        {
            ArgumentNullException.ThrowIfNull(values);
            var data = values.ToList();
            if (data.Count < 4) throw new ArgumentException("Kurtosis requires at least 4 values");

            double mean = Mean(data);
            double stdDev = SampleStandardDeviation(data);
            double n = data.Count;

            double kurt = data.Sum(x => Math.Pow((x - mean) / stdDev, 4)) * n * (n + 1) / ((n - 1) * (n - 2) * (n - 3))
                          - 3 * (n - 1) * (n - 1) / ((n - 2) * (n - 3));
            return kurt;
        }

        #endregion

        #region Correlation and Regression

        /// <summary>
        /// Calculates the Pearson correlation coefficient between two datasets.
        /// </summary>
        public static double PearsonCorrelation(IEnumerable<double> x, IEnumerable<double> y)
        {
            ArgumentNullException.ThrowIfNull(x);
            ArgumentNullException.ThrowIfNull(y);

            var xData = x.ToList();
            var yData = y.ToList();

            if (xData.Count != yData.Count) throw new ArgumentException("Datasets must have the same length");
            if (xData.Count < 2) throw new ArgumentException("Correlation requires at least 2 pairs of values");

            double xMean = Mean(xData);
            double yMean = Mean(yData);

            double numerator = xData.Zip(yData, (xi, yi) => (xi - xMean) * (yi - yMean)).Sum();
            double xSumSq = xData.Sum(xi => Math.Pow(xi - xMean, 2));
            double ySumSq = yData.Sum(yi => Math.Pow(yi - yMean, 2));

            double denominator = Math.Sqrt(xSumSq * ySumSq);
            return denominator == 0 ? 0 : numerator / denominator;
        }

        /// <summary>
        /// Calculates the Spearman rank correlation coefficient between two datasets.
        /// </summary>
        public static double SpearmanCorrelation(IEnumerable<double> x, IEnumerable<double> y)
        {
            ArgumentNullException.ThrowIfNull(x);
            ArgumentNullException.ThrowIfNull(y);

            var xRanks = GetRanks(x);
            var yRanks = GetRanks(y);

            return PearsonCorrelation(xRanks, yRanks);
        }

        /// <summary>
        /// Performs simple linear regression and returns slope and intercept.
        /// </summary>
        public static (double slope, double intercept, double rSquared) LinearRegression(IEnumerable<double> x, IEnumerable<double> y)
        {
            ArgumentNullException.ThrowIfNull(x);
            ArgumentNullException.ThrowIfNull(y);

            var xData = x.ToList();
            var yData = y.ToList();

            if (xData.Count != yData.Count) throw new ArgumentException("Datasets must have the same length");
            if (xData.Count < 2) throw new ArgumentException("Linear regression requires at least 2 pairs of values");

            double xMean = Mean(xData);
            double yMean = Mean(yData);

            double numerator = xData.Zip(yData, (xi, yi) => (xi - xMean) * (yi - yMean)).Sum();
            double denominator = xData.Sum(xi => Math.Pow(xi - xMean, 2));

            if (denominator == 0) throw new ArgumentException("Cannot perform regression with constant x values");

            double slope = numerator / denominator;
            double intercept = yMean - slope * xMean;

            // Calculate R-squared
            var predicted = xData.Select(xi => slope * xi + intercept);
            double ssRes = yData.Zip(predicted, (actual, pred) => Math.Pow(actual - pred, 2)).Sum();
            double ssTot = yData.Sum(yi => Math.Pow(yi - yMean, 2));
            double rSquared = ssTot == 0 ? 1 : 1 - (ssRes / ssTot);

            return (slope, intercept, rSquared);
        }

        private static double[] GetRanks(IEnumerable<double> values)
        {
            var indexed = values.Select((value, index) => new { Value = value, Index = index }).ToList();
            var sorted = indexed.OrderBy(x => x.Value).ToList();

            var ranks = new double[indexed.Count];

            for (int i = 0; i < sorted.Count; i++)
            {
                int j = i;
                while (j < sorted.Count - 1 && sorted[j].Value == sorted[j + 1].Value)
                    j++;

                double rank = (i + j + 2) / 2.0; // Average rank for ties

                for (int k = i; k <= j; k++)
                    ranks[sorted[k].Index] = rank;

                i = j;
            }

            return ranks;
        }

        #endregion

        #region Probability Distributions

        /// <summary>
        /// Calculates the probability density function of the normal distribution.
        /// </summary>
        public static double NormalPDF(double x, double mean = 0, double stdDev = 1)
        {
            if (stdDev <= 0) throw new ArgumentException("Standard deviation must be positive");

            double z = (x - mean) / stdDev;
            return Math.Exp(-0.5 * z * z) / (stdDev * SQRT_2PI);
        }

        /// <summary>
        /// Calculates the cumulative distribution function of the standard normal distribution.
        /// </summary>
        public static double StandardNormalCDF(double z)
        {
            // Abramowitz and Stegun approximation
            double a1 = 0.254829592;
            double a2 = -0.284496736;
            double a3 = 1.421413741;
            double a4 = -1.453152027;
            double a5 = 1.061405429;
            double p = 0.3275911;

            int sign = z < 0 ? -1 : 1;
            z = Math.Abs(z);

            double t = 1.0 / (1.0 + p * z);
            double y = 1.0 - (((((a5 * t + a4) * t) + a3) * t + a2) * t + a1) * t * Math.Exp(-z * z);

            return 0.5 * (1.0 + sign * y);
        }

        /// <summary>
        /// Calculates the cumulative distribution function of the normal distribution.
        /// </summary>
        public static double NormalCDF(double x, double mean = 0, double stdDev = 1)
        {
            if (stdDev <= 0) throw new ArgumentException("Standard deviation must be positive");
            return StandardNormalCDF((x - mean) / stdDev);
        }

        /// <summary>
        /// Calculates the inverse of the standard normal cumulative distribution function.
        /// </summary>
        public static double StandardNormalInverse(double p)
        {
            if (p <= 0 || p >= 1) throw new ArgumentException("Probability must be between 0 and 1 (exclusive)");

            // Beasley-Springer-Moro algorithm
            double[] a = [ 0, -3.969683028665376e+01, 2.209460984245205e+02, -2.759285104469687e+02,
                       1.383577518672690e+02, -3.066479806614716e+01, 2.506628277459239e+00 ];

            double[] b = [ 0, -5.447609879822406e+01, 1.615858368580409e+02, -1.556989798598866e+02,
                       6.680131188771972e+01, -1.328068155288572e+01 ];

            double[] c = [ 0, -7.784894002430293e-03, -3.223964580411365e-01, -2.400758277161838e+00,
                       -2.549732539343734e+00, 4.374664141464968e+00, 2.938163982698783e+00 ];

            double[] d = [ 0, 7.784695709041462e-03, 3.224671290700398e-01, 2.445134137142996e+00,
                       3.754408661907416e+00 ];

            double q, t, u;

            if (p < 0.02425)
            {
                q = Math.Sqrt(-2 * Math.Log(p));
                return (((((c[1] * q + c[2]) * q + c[3]) * q + c[4]) * q + c[5]) * q + c[6]) /
                       ((((d[1] * q + d[2]) * q + d[3]) * q + d[4]) * q + 1);
            }

            if (p < 0.97575)
            {
                q = p - 0.5;
                t = q * q;
                u = (((((a[1] * t + a[2]) * t + a[3]) * t + a[4]) * t + a[5]) * t + a[6]) * q /
                    (((((b[1] * t + b[2]) * t + b[3]) * t + b[4]) * t + b[5]) * t + 1);
                return u;
            }

            q = Math.Sqrt(-2 * Math.Log(1 - p));
            return -(((((c[1] * q + c[2]) * q + c[3]) * q + c[4]) * q + c[5]) * q + c[6]) /
                   ((((d[1] * q + d[2]) * q + d[3]) * q + d[4]) * q + 1);
        }

        /// <summary>
        /// Calculates the probability density function of the t-distribution.
        /// </summary>
        public static double TDistributionPDF(double t, int degreesOfFreedom)
        {
            if (degreesOfFreedom <= 0) throw new ArgumentException("Degrees of freedom must be positive");

            double gamma1 = LogGamma((degreesOfFreedom + 1.0) / 2.0);
            double gamma2 = LogGamma(degreesOfFreedom / 2.0);
            double logPdf = gamma1 - gamma2 - 0.5 * Math.Log(degreesOfFreedom * Math.PI)
                            - ((degreesOfFreedom + 1.0) / 2.0) * Math.Log(1.0 + t * t / degreesOfFreedom);

            return Math.Exp(logPdf);
        }

        /// <summary>
        /// Calculates the cumulative distribution function of the chi-squared distribution.
        /// </summary>
        public static double ChiSquaredCDF(double x, int degreesOfFreedom)
        {
            if (x < 0) return 0;
            if (degreesOfFreedom <= 0) throw new ArgumentException("Degrees of freedom must be positive");

            return IncompleteGamma(degreesOfFreedom / 2.0, x / 2.0);
        }

        // Helper method for log gamma function
        private static double LogGamma(double x)
        {
            // Lanczos approximation
            double[] coeff = [
            0.99999999999980993,
            676.5203681218851,
            -1259.1392167224028,
            771.32342877765313,
            -176.61502916214059,
            12.507343278686905,
            -0.13857109526572012,
            9.9843695780195716e-6,
            1.5056327351493116e-7
        ];

            if (x < 0.5)
                return Math.Log(Math.PI / Math.Sin(Math.PI * x)) - LogGamma(1 - x);

            x -= 1;
            double a = coeff[0];
            for (int i = 1; i < coeff.Length; i++)
                a += coeff[i] / (x + i);

            double t = x + coeff.Length - 1.5;
            return 0.5 * Math.Log(2 * Math.PI) + (x + 0.5) * Math.Log(t) - t + Math.Log(a);
        }

        // Helper method for incomplete gamma function
        private static double IncompleteGamma(double a, double x)
        {
            if (x == 0) return 0;
            if (x < a + 1)
            {
                // Use series representation
                double sum = 1.0 / a;
                double term = 1.0 / a;
                for (int n = 1; n < 100; n++)
                {
                    term *= x / (a + n);
                    sum += term;
                    if (Math.Abs(term) < 1e-15) break;
                }
                return Math.Exp(-x + a * Math.Log(x) - LogGamma(a)) * sum;
            }
            else
            {
                // Use continued fraction
                double b = x + 1 - a;
                double c = 1e30;
                double d = 1 / b;
                double h = d;

                for (int i = 1; i <= 100; i++)
                {
                    double an = -i * (i - a);
                    b += 2;
                    d = an * d + b;
                    if (Math.Abs(d) < 1e-30) d = 1e-30;
                    c = b + an / c;
                    if (Math.Abs(c) < 1e-30) c = 1e-30;
                    d = 1 / d;
                    double del = d * c;
                    h *= del;
                    if (Math.Abs(del - 1) < 1e-15) break;
                }

                double gammaCF = Math.Exp(-x + a * Math.Log(x) - LogGamma(a)) * h;
                return 1 - gammaCF;
            }
        }

        #endregion

        #region Hypothesis Testing

        /// <summary>
        /// Performs a one-sample t-test.
        /// </summary>
        public static (double tStatistic, double pValue, bool isSignificant) OneSampleTTest(
            IEnumerable<double> sample, double populationMean, double alpha = 0.05)
        {
            ArgumentNullException.ThrowIfNull(sample);
            var data = sample.ToList();
            if (data.Count < 2) throw new ArgumentException("Sample must contain at least 2 values");

            double sampleMean = Mean(data);
            double sampleStdDev = SampleStandardDeviation(data);
            int n = data.Count;

            double tStatistic = (sampleMean - populationMean) / (sampleStdDev / Math.Sqrt(n));
            double pValue = 2 * (1 - TDistributionCDF(Math.Abs(tStatistic), n - 1));
            bool isSignificant = pValue < alpha;

            return (tStatistic, pValue, isSignificant);
        }

        /// <summary>
        /// Performs a two-sample t-test assuming equal variances.
        /// </summary>
        public static (double tStatistic, double pValue, bool isSignificant) TwoSampleTTest(
            IEnumerable<double> sample1, IEnumerable<double> sample2, double alpha = 0.05)
        {
            ArgumentNullException.ThrowIfNull(sample1);
            ArgumentNullException.ThrowIfNull(sample2);

            var data1 = sample1.ToList();
            var data2 = sample2.ToList();

            if (data1.Count < 2 || data2.Count < 2)
                throw new ArgumentException("Both samples must contain at least 2 values");

            double mean1 = Mean(data1);
            double mean2 = Mean(data2);
            double var1 = SampleVariance(data1);
            double var2 = SampleVariance(data2);
            int n1 = data1.Count;
            int n2 = data2.Count;

            // Pooled variance
            double pooledVar = ((n1 - 1) * var1 + (n2 - 1) * var2) / (n1 + n2 - 2);
            double standardError = Math.Sqrt(pooledVar * (1.0 / n1 + 1.0 / n2));

            double tStatistic = (mean1 - mean2) / standardError;
            int degreesOfFreedom = n1 + n2 - 2;
            double pValue = 2 * (1 - TDistributionCDF(Math.Abs(tStatistic), degreesOfFreedom));
            bool isSignificant = pValue < alpha;

            return (tStatistic, pValue, isSignificant);
        }

        /// <summary>
        /// Performs a chi-squared goodness of fit test.
        /// </summary>
        public static (double chiSquaredStatistic, double pValue, bool isSignificant) ChiSquaredGoodnessOfFit(
            IEnumerable<double> observed, IEnumerable<double> expected, double alpha = 0.05)
        {
            ArgumentNullException.ThrowIfNull(observed);
            ArgumentNullException.ThrowIfNull(expected);

            var obs = observed.ToList();
            var exp = expected.ToList();

            if (obs.Count != exp.Count) throw new ArgumentException("Observed and expected must have same length");
            if (obs.Count == 0) throw new ArgumentException("Arrays cannot be empty");
            if (exp.Any(e => e <= 0)) throw new ArgumentException("Expected frequencies must be positive");

            double chiSquared = obs.Zip(exp, (o, e) => Math.Pow(o - e, 2) / e).Sum();
            int degreesOfFreedom = obs.Count - 1;
            double pValue = 1 - ChiSquaredCDF(chiSquared, degreesOfFreedom);
            bool isSignificant = pValue < alpha;

            return (chiSquared, pValue, isSignificant);
        }

        // Approximation for t-distribution CDF using normal approximation for large df
        private static double TDistributionCDF(double t, int df)
        {
            if (df > 30)
                return StandardNormalCDF(t); // Normal approximation for large df

            // For small df, use a simple approximation
            // This is not as accurate as the exact calculation but sufficient for most purposes
            double x = df / (df + t * t);
            return 0.5 + (t / Math.Sqrt(df)) * BetaFunction(0.5, df / 2.0) *
                   Math.Pow(x, df / 2.0) / (2 * BetaFunction(0.5, 0.5));
        }

        // Simple beta function approximation
        private static double BetaFunction(double a, double b)
        {
            return Math.Exp(LogGamma(a) + LogGamma(b) - LogGamma(a + b));
        }

        #endregion

        #region Summary Statistics

        /// <summary>
        /// Generates comprehensive descriptive statistics for a dataset.
        /// </summary>
        public static DescriptiveStatistics GetDescriptiveStatistics(IEnumerable<double> values)
        {
            ArgumentNullException.ThrowIfNull(values);
            var data = values.ToList();
            if (data.Count == 0) throw new ArgumentException("Dataset cannot be empty");

            return new DescriptiveStatistics
            {
                Count = data.Count,
                Mean = Mean(data),
                Median = Median(data),
                Mode = [.. Mode(data)],
                StandardDeviation = data.Count > 1 ? SampleStandardDeviation(data) : 0,
                Variance = data.Count > 1 ? SampleVariance(data) : 0,
                Range = Range(data),
                Minimum = data.Min(),
                Maximum = data.Max(),
                Q1 = Percentile(data, 25),
                Q3 = Percentile(data, 75),
                InterquartileRange = data.Count > 1 ? InterquartileRange(data) : 0,
                Skewness = data.Count > 2 ? Skewness(data) : 0,
                Kurtosis = data.Count > 3 ? Kurtosis(data) : 0
            };
        }

        #endregion
    }

    /// <summary>
    /// Contains comprehensive descriptive statistics for a dataset.
    /// </summary>
    public class DescriptiveStatistics
    {
        public int Count { get; set; }
        public double Mean { get; set; }
        public double Median { get; set; }
        public List<double> Mode { get; set; } = [];
        public double StandardDeviation { get; set; }
        public double Variance { get; set; }
        public double Range { get; set; }
        public double Minimum { get; set; }
        public double Maximum { get; set; }
        public double Q1 { get; set; }
        public double Q3 { get; set; }
        public double InterquartileRange { get; set; }
        public double Skewness { get; set; }
        public double Kurtosis { get; set; }

        public override string ToString()
        {
            return $@"Descriptive Statistics:
Count: {Count}
Mean: {Mean:F4}
Median: {Median:F4}
Mode: {string.Join(", ", Mode.Select(m => m.ToString("F4")))}
Standard Deviation: {StandardDeviation:F4}
Variance: {Variance:F4}
Range: {Range:F4}
Minimum: {Minimum:F4}
Maximum: {Maximum:F4}
Q1: {Q1:F4}
Q3: {Q3:F4}
IQR: {InterquartileRange:F4}
Skewness: {Skewness:F4}
Kurtosis: {Kurtosis:F4}";
        }
    }

    // Example usage class
    public class StatisticsExample
    {
        public static void Main()
        {
            // Example dataset
            var data = new double[] { 1.2, 2.3, 3.4, 4.5, 5.6, 6.7, 7.8, 8.9, 9.0, 10.1 };

            // Basic descriptive statistics
            Console.WriteLine("=== Descriptive Statistics ===");
            var stats = Statistics.GetDescriptiveStatistics(data);
            Console.WriteLine(stats);

            // Correlation example
            var x = new double[] { 1, 2, 3, 4, 5 };
            var y = new double[] { 2, 4, 6, 8, 10 };
            Console.WriteLine($"\nPearson Correlation: {Statistics.PearsonCorrelation(x, y):F4}");

            // Linear regression example
            var (slope, intercept, rSquared) = Statistics.LinearRegression(x, y);
            Console.WriteLine($"Linear Regression: y = {slope:F4}x + {intercept:F4}, R² = {rSquared:F4}");

            // Hypothesis testing example
            var (tStat, pValue, significant) = Statistics.OneSampleTTest(data, 5.0);
            Console.WriteLine($"\nOne-sample t-test (μ₀ = 5.0):");
            Console.WriteLine($"t-statistic: {tStat:F4}");
            Console.WriteLine($"p-value: {pValue:F4}");
            Console.WriteLine($"Significant at α = 0.05: {significant}");

            // Normal distribution example
            Console.WriteLine($"\nNormal PDF(0, 0, 1): {Statistics.NormalPDF(0):F4}");
            Console.WriteLine($"Normal CDF(1.96, 0, 1): {Statistics.NormalCDF(1.96):F4}");

            // Two-sample t-test example
            var sample1 = new double[] { 1.1, 2.2, 3.3, 4.4, 5.5 };
            var sample2 = new double[] { 2.1, 3.2, 4.3, 5.4, 6.5 };
            var (tStat2, pValue2, sig2) = Statistics.TwoSampleTTest(sample1, sample2);
            Console.WriteLine($"\nTwo-sample t-test:");
            Console.WriteLine($"t-statistic: {tStat2:F4}");
            Console.WriteLine($"p-value: {pValue2:F4}");
            Console.WriteLine($"Significant at α = 0.05: {sig2}");
        }
    }
}
