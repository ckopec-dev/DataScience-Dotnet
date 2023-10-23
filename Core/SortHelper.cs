namespace Core
{
    /// <summary>
    /// These are mainly for demonstration purposes. Native sorting is generally much more efficient.
    /// </summary>
    public static class SortHelper
    {
        #region Public methods

        public static void BubbleSort(int[] data)
        {
            bool swapMade = true;

            while (swapMade)
            {
                swapMade = false;

                for (int i = 0; i < data.Length - 1; i++)
                {
                    if (data[i + 1] < data[i])
                    {
                        (data[i + 1], data[i]) = (data[i], data[i + 1]);
                        swapMade = true;
                    }
                }
            }
        }

        public static void ShellSort(int[] data)
        {
            int gap = (int)Math.Floor((decimal)data.Length / 2m);

            if (gap == 0)
                gap = 1;

            bool swapMade = true;

            while (swapMade)
            {
                int interval = data.Length - gap;

                swapMade = false;

                for (int i = 0; i < interval; i++)
                {
                    if (data[i + gap] < data[i])
                    {
                        (data[i + gap], data[i]) = (data[i], data[i + gap]);
                        swapMade = true;

                        gap = (int)Math.Floor((decimal)gap / 2m);

                        if (gap == 0)
                            gap = 1;
                    }
                }
            }
        }

        #endregion

        #region Helper methods

        #endregion
    }
}
