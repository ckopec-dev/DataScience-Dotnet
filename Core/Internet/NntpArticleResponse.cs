using System.Text;

namespace Core.Internet
{
    public class NntpArticleResponse : NntpResponse
    {
        public int? ArticleNumber { get; set; }
        public string? MessageId { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }

        public NntpArticleResponse()
        {
        }

        public NntpArticleResponse(int articleNumber, 
            string messageId, string? subject, string? body)
        {
            ArticleNumber = articleNumber;  
            MessageId = messageId;
            Subject = subject;
            Body = body;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"{ResponseCode} {ArticleNumber} {MessageId}");

            sb.AppendLine($"Subject: {Subject}");
            sb.AppendLine($"Body: {Body}");
        
            return sb.ToString().Trim();
        }
    }
}
