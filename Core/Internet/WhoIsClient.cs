using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Core.Internet
{
    public class WhoIsClient
    {
        public static void Query()
        {
            string domain = "yahoo.com";

            try
            {
                string whoisServer = "whois.verisign-grs.com"; // For .com/.net domains
                int port = 43;

                using (TcpClient client = new TcpClient(whoisServer, port))
                using (NetworkStream stream = client.GetStream())
                using (StreamWriter writer = new StreamWriter(stream))
                using (StreamReader reader = new StreamReader(stream))
                {
                    writer.WriteLine(domain);
                    writer.Flush();

                    string response = reader.ReadToEnd();
                    Console.WriteLine("WHOIS Response:\n");
                    Console.WriteLine(response);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
