
using NLog;
using System.Text;

namespace Core.Internet
{
    public class NntpResponse
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public bool Success { get; set; }
        public string? Exception { get; set; }
        public string? RawResponse { get; set; }

        public NntpResponseCode? ResponseCode 
        { 
            get
            {
                if (RawResponse == null)
                {
                    throw new NntpUnknownResponseCodeException();
                }
                else
                {
                    try
                    {
                        int code = Convert.ToInt32(RawResponse[..3]);
                        return (NntpResponseCode)code;
                    }
                    catch
                    {
                        throw new NntpUnknownResponseCodeException();
                    }
                }
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine("\nSuccess: " + Success);
            sb.AppendLine("ResponseCode: " + ResponseCode);
            if (RawResponse != null)
                sb.AppendLine("RawResponse: " + RawResponse);
            if (Exception != null)
                sb.AppendLine("Exception: " + Exception);

            return sb.ToString().Trim();
        }
    }
}
