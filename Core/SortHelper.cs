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

        #endregion

        #region Helper methods

        #endregion
    }
}
