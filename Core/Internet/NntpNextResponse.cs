
using System.Reflection.PortableExecutable;
using System.Text;

namespace Core.Internet
{
    public class NntpNextResponse : NntpResponse
    {
        public int? ResponseCode { get; set; }
        public int? ArticleNumber { get; set; }
        public string? MessageId { get; set; }

        public NntpNextResponse() 
        { 
        }

        public NntpNextResponse(int? responseCode, int? articleNumber, string? messageId)
        {
            ResponseCode = responseCode;
            ArticleNumber = articleNumber;
            MessageId = messageId;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"{ResponseCode} {ArticleNumber} {MessageId}");

            return sb.ToString().Trim();
        }
    }
}
