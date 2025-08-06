
namespace Core.Internet
{
    public class NntpListResponse : NntpResponse
    {
        public List<NntpListResponseItem> Items { get; set; } = [];
    }
}
