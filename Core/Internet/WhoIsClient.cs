using System.Net.Sockets;

namespace Core.Internet
{
    public class WhoIsClient
    {
        public static string Query(
            string domain, 
            string server = "whois.verisign-grs.com",
            int port = 43)
        {
            using TcpClient client = new(server, port);
            using NetworkStream stream = client.GetStream();
            using StreamWriter writer = new(stream);
            using StreamReader reader = new(stream);
            writer.WriteLine(domain);
            writer.Flush();

            string response = reader.ReadToEnd();
            return response;
        }
    }
}
