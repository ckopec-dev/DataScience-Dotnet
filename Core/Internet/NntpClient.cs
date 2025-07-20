using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace Core.Internet
{
    public class NntpClient
    {
        private TcpClient? tcpClient;
        private StreamReader? reader;
        private StreamWriter? writer;

        public void Connect(string server, int port = 119)
        {
            tcpClient = new TcpClient(server, port);
            var stream = tcpClient.GetStream();
            reader = new StreamReader(stream, Encoding.ASCII);
            writer = new StreamWriter(stream, Encoding.ASCII) { AutoFlush = true };

            Console.WriteLine("Server: " + reader.ReadLine());
        }

        public void SendCommand(string command)
        {
            if (writer == null) throw new DisconnectedException();
            
            Console.WriteLine("Client: " + command);
            writer.WriteLine(command);
        }

        public void ReadResponse(bool multiline = false)
        {
            if (reader == null) throw new DisconnectedException();
            
            string? line;
            do
            {
                line = reader.ReadLine();
                Console.WriteLine("Server: " + line);
            } while (multiline && line != "." && line != null);
        }

        public void ListNewsgroups()
        {
            SendCommand("LIST");
            ReadResponse(multiline: true);
        }

        public void SelectNewsgroup(string group)
        {
            SendCommand($"GROUP {group}");
            ReadResponse();
        }

        public void GetArticle(int articleNumber)
        {
            SendCommand($"ARTICLE {articleNumber}");
            ReadResponse(multiline: true);
        }

        public void Quit()
        {
            if (tcpClient == null) throw new DisconnectedException();
            
            SendCommand("QUIT");
            ReadResponse();
            tcpClient.Close();
        }
    }
}
