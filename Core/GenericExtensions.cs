
namespace Core
{
    public static class GenericExtensions
    {
        private static readonly Random rnd = new();

        public static void RandomSort<T>(this IList<T> list)
        {
            int n = list.Count;

            while (n > 1)
            {
                n--;

                int k = rnd.Next(n + 1);

                (list[n], list[k]) = (list[k], list[n]);
            }
        }
    }
}
