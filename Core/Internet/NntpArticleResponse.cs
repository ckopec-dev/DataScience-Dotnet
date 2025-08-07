namespace Core.Internet
{
    public class NntpArticleResponse : NntpResponse
    {
        public string? Header { get; set; }
        public string? Body { get; set; }

        public NntpArticleResponse() 
        { 
        }

        public NntpArticleResponse(string? header, string? body)
        {
            Header = header;
            Body = body;
        }

        public override string ToString()
        {
            return String.Format($"\n\rHeader: {Header}" + Environment.NewLine + $"Body: {Body}");
        }  
    }
}
