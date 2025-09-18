namespace Core.Maths
{
    public class LinearRegression
    {
        public static decimal Slope(decimal[] xVals, decimal[] yVals)
        {
            if (xVals.Length != yVals.Length)
                throw new Exception("Arguments must be identical length.");
            
            decimal n = 0, d = 0;
            decimal avgX = xVals.Average();
            decimal avgY = yVals.Average();

            for(int i = 0; i < xVals.Length; i++)
            {
                n += (xVals[i] - avgX) * (yVals[i] - avgY);
                d += (xVals[i] - avgX) * (xVals[i] - avgX);
            }

            return n / d;
        }

        public static decimal Intercept(decimal[] xVals, decimal[] yVals)
        {
            return yVals.Average() - Slope(xVals, yVals) * xVals.Average();
        }

        public static decimal Intercept(decimal[] xVals, decimal[] yVals, decimal slope)
        {
            // More efficient if slope has already been calculated.
            return yVals.Average() - slope * xVals.Average();
        }

        public static decimal Predict(decimal x, decimal slope, decimal intercept)
        {
            return slope * x + intercept;
        }
    }
}
