﻿namespace Core
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

        public static void HeapSort(int[] data)
        {
            HeapSort(data, data.Length);
        }

        public static void HeapSort(int[] arr, int n)
        {
            for (int i = n / 2 - 1; i >= 0; i--)
                Heapify(arr, n, i);

            for (int i = n - 1; i >= 0; i--)
            {
                (arr[i], arr[0]) = (arr[0], arr[i]);
                Heapify(arr, i, 0);
            }
        }

        public static void SelectionSort(int[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                int lowestJ = int.MaxValue;
                int lowestJIdx = 0;

                for (int j = i; j < data.Length; j++)
                {
                    // Find the lowest value of j. Update i with that value.

                    if (data[j] < lowestJ)
                    {
                        lowestJ = data[j];
                        lowestJIdx = j;
                    }
                }

                (data[lowestJIdx], data[i]) = (data[i], data[lowestJIdx]);
            }

        }

        public static void QuickSort(int[] data)
        {
            QuickSort(data, 0, data.Length - 1);
        }

        public static void QuickSort(int[] data, int left, int right)
        {
            if (left < right)
            {
                int pivot = Partition(data, left, right);

                if (pivot > 1)
                {
                    QuickSort(data, left, pivot - 1);
                }
                if (pivot + 1 < right)
                {
                    QuickSort(data, pivot + 1, right);
                }
            }
        }

        #endregion

        #region Helper methods

        private static void Heapify(int[] arr, int n, int i)
        {
            int largest = i;
            int left = 2 * i + 1;
            int right = 2 * i + 2;

            if (left < n && arr[left] > arr[largest])
                largest = left;

            if (right < n && arr[right] > arr[largest])
                largest = right;

            if (largest != i)
            {
                (arr[largest], arr[i]) = (arr[i], arr[largest]);
                Heapify(arr, n, largest);
            }
        }

        private static int Partition(int[] arr, int left, int right)
        {
            int pivot = arr[left];
            while (true)
            {

                while (arr[left] < pivot)
                {
                    left++;
                }

                while (arr[right] > pivot)
                {
                    right--;
                }

                if (left < right)
                {
                    if (arr[left] == arr[right]) return right;

                    (arr[right], arr[left]) = (arr[left], arr[right]);
                }
                else
                {
                    return right;
                }
            }
        }

        #endregion
    }
}
