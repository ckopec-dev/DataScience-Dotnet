
using System.Text;

namespace Core.Internet
{
    public class NntpListResponse : NntpResponse
    {
        public List<NntpListResponseItem> Items { get; set; } = [];

        public override string ToString()
        {
            StringBuilder sb = new();
            
            foreach (NntpListResponseItem item in Items)
            {
                sb.AppendLine(item.ToString());
            }

            return sb.ToString().Trim();
        }
    }
}
