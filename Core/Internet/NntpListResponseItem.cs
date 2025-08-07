
namespace Core.Internet
{
    public class NntpListResponseItem(string name, int low, int high, 
        bool? postingAllowed)
    {
        public string Name { get; set; } = name;
        public int Low { get; set; } = low;
        public int High { get; set; } = high;
        public bool? PostingAllowed { get; set; } = postingAllowed;

        public override string ToString()
        {
            return String.Format("Name: {0}, Low: {1}, High: {2}, Posting allowed: {3}",
                Name, Low, High, PostingAllowed);
        }
    }
}
