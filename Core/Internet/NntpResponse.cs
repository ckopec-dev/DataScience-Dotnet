
namespace Core.Internet
{
    public class NntpResponse
    {
        public bool Success { get; set; }
        public string? Response { get; set; }
        public List<string> MultilineResponse { get; set; } = [];

        public string? RawResponse
        {
            get
            {
                string? s = Response + Environment.NewLine;

                foreach(string line in MultilineResponse)
                {
                    s += line + Environment.NewLine;
                }

                return s;
            }
        }

        public NntpResponse()
        {
        }

        public NntpResponse(bool success, string? response)
        {
            Success = success;
            Response = response;
        }

        public NntpResponse(bool success, List<string> multilineResponse)
        {
            Success = success;
            MultilineResponse = multilineResponse;
        }
    }
}
