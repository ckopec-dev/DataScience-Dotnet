
namespace Core.Internet
{
    public class NntpResponse
    {
        public bool Success { get; set; }
        public string? Response { get; set; }
        public List<string> MultilineResponse { get; set; } = [];

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
