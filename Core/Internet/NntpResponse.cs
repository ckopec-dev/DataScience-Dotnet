
namespace Core.Internet
{
    public class NntpResponse
    {
        public bool Success { get; set; }
        public string? Exception { get; set; }
        public string? RawResponse { get; set; }
    }
}
