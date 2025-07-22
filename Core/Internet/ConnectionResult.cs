
namespace Core.Internet
{
    public class ConnectionResult
    {
        public bool Success { get; set; }
        public string? Response { get; set; }

        public ConnectionResult()
        {
        }

        public ConnectionResult(bool success, string? response)
        {
            Success = success;
            Response = response;
        }
    }
}
