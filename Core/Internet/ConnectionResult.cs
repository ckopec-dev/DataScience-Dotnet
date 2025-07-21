
namespace Core.Internet
{
    public class ConnectionResult(bool success, string response)
    {
        public bool Success { get; set; } = success;
        public string Response { get; set; } = response;
    }
}
