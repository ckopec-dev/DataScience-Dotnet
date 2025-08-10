using System.Text;

namespace Core.Internet
{
    public class NntpArticleResponse : NntpResponse
    {
        public int? ArticleNumber { get; set; }
        public string? MessageId { get; set; }
        public string? Header { get; set; }
        public string? Body { get; set; }

        public NntpArticleResponse() 
        { 
        }

        public NntpArticleResponse(int? articleNumber, 
            string? messageId, string? header, string? body)
        {
            ArticleNumber = articleNumber;  
            MessageId = messageId;
            Header = header;
            Body = body;
        }

        public override string ToString()
        {
            return ToString(false);
        }

        public string ToString(bool verbose)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"{ResponseCode} {ArticleNumber} {MessageId}");

            if (verbose)
            {
                sb.AppendLine($"Header: {Header}");
                sb.AppendLine($"Body: {Body}");
            }

            return sb.ToString().Trim();
        }  
    }
}
