namespace Core.Internet
{
    public class NntpArticleResponse : NntpResponse
    {
        public string? Header { get; set; }
        public string? Body { get; set; }
    }
}
