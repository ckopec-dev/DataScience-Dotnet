using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace Core.Internet
{
    public class NntpClient
    {
        #region Fields 

        private TcpClient? tcpClient;
        private StreamReader? reader;
        private StreamWriter? writer;

        #endregion

        #region Methods

        public string Connect(string server, int port = 119)
        {
            // Returns server response message.

            tcpClient = new TcpClient(server, port);
            var stream = tcpClient.GetStream();
            reader = new StreamReader(stream, Encoding.ASCII);
            writer = new StreamWriter(stream, Encoding.ASCII) { AutoFlush = true };

            string? response = reader.ReadLine() ?? throw new DisconnectedException();
            Console.WriteLine("SERVER: {0}", response);

            return response;
        }

        public void Authenticate(string username, string password)
        {
            SendCommand("AUTHINFO USER " + username);
            ReadResponse();

            SendCommand("AUTHINFO PASS " + password);
            ReadResponse();
        }

        private void SendCommand(string command)
        {
            if (writer == null) throw new DisconnectedException();
            
            Console.WriteLine("CLIENT: " + command);
            writer.WriteLine(command);
        }

        public List<string> ReadResponse(bool multiline = false)
        {
            if (reader == null) throw new DisconnectedException();
            List<string> response = [];

            string? line;
            do
            {
                line = reader.ReadLine();
                if (line != null)
                {
                    response.Add(line);
                    Console.WriteLine("SERVER: " + line);
                }

            } while (multiline && line != "." && line != null);

            return response;
        }

        private static List<string> Filter(List<string> response, List<string> excludes)
        {
            List<string> retval = [];

            foreach (var item in response)
            {
                bool add = true;

                foreach (var ex in excludes)
                {
                    bool match = item.StartsWith(ex);
                    Console.WriteLine("{0} starts with {1}? {2}", item, ex, match);
                    if (match)
                    {
                        add = false;
                        break;
                    }
                }

                if (add)
                    retval.Add(item);
            }

            return retval;
        }

        public void Quit()
        {
            if (tcpClient == null) throw new DisconnectedException();

            SendCommand("QUIT");
            ReadResponse();
            tcpClient.Close();
        }

        public List<string> ListNewsgroups()
        {
            SendCommand("LIST");
            List<string> response = ReadResponse(multiline: true);
            response = Filter(response, ["215", "."]);

            return response;
        }

        public List<string> GetArticles(string group)
        {
            SendCommand($"LISTGROUP {group}");
            List<string> response = ReadResponse(multiline: true);

            return response;
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

        #endregion
    }
}
