namespace Core.Maths
{
    public class Histogram<TVal> : SortedDictionary<TVal, uint> where TVal : notnull 
    {
        public void Increment(TVal bin)
        {
            if (ContainsKey(bin))
            {
                this[bin]++;
            }
            else
            {
                Add(bin, 1);
            }
        }
    }
}
