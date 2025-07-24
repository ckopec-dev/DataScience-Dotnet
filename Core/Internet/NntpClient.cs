using System.Net.Sockets;
using System.Text;

namespace Core.Internet
{
    public class NntpClient
    {
        #region Fields 

        private const int TIMEOUT = 3000;       // In milliseconds
        private readonly bool _verbose = false;
        private TcpClient? tcpClient;
        private StreamReader? reader;
        private StreamWriter? writer;

        #endregion

        #region Ctors/Dtors 

        public NntpClient()
        {
        }

        public NntpClient(bool verbose)
        {
            _verbose = verbose;
        }

        #endregion

        #region Methods

        public NntpResponse Connect(string server, int port = 119)
        {
            NntpResponse r = new();

            try
            {
                tcpClient = new TcpClient();
                
                if (!tcpClient.ConnectAsync(server, port).Wait(TIMEOUT))
                {
                    r.Success = false;
                    r.Response = "Connection timeout.";
                    return r;
                }
                
                NetworkStream stream = tcpClient.GetStream();
                stream.ReadTimeout = TIMEOUT;

                reader = new StreamReader(stream, Encoding.ASCII);
                writer = new StreamWriter(stream, Encoding.ASCII) { AutoFlush = true };

                string? response = reader.ReadLine();

                r.Success = true;
                r.Response = response;

                if (_verbose)
                    Console.WriteLine("SERVER: {0}", response);
            }
            catch(Exception ex)
            {
                r.Success = false;
                r.Response = "EXCEPTION CAUGHT: " + ex.ToString();
            }

            return r;
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

            if (_verbose)
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

                    if (_verbose)
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

        public bool Quit()
        {
            try
            {
                if (tcpClient == null)
                    return false;

                SendCommand("QUIT");
                ReadResponse();
                tcpClient.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public NntpResponse GetNewsgroups()
        {
            NntpResponse r = new();

            try
            {
                SendCommand("LIST");
                List<string> response = ReadResponse(multiline: true);
                r.Success = true;
                r.MultilineResponse = Filter(response, ["215", "."]);
            }
            catch(Exception ex)
            {
                r.Success = false;
                r.Response = "EXCEPTION CAUGHT: " + ex.ToString();
            }

            return r;
        }

        public NntpResponse GetArticles(string group)
        {
            NntpResponse r = new();

            try
            {
                SendCommand($"LISTGROUP {group}");
                List<string> response = ReadResponse(multiline: true);
                r.Success = true;
                r.MultilineResponse = Filter(response, ["."]);
            }
            catch(Exception ex)
            {
                r.Success = false;
                r.Response = "EXCEPTION CAUGHT: " + ex.ToString();
            }

            return r;
        }

        public NntpResponse SelectNewsgroup(string group)
        {
            NntpResponse r = new();

            try
            {
                SendCommand($"GROUP {group}");
                List<string> response = ReadResponse();
                r.Success = true;

                if (response.Count == 1)
                {
                    r.Response = response[0];
                }
                else
                {
                    r.Success = false;
                }
            }
            catch(Exception ex)
            {
                r.Success = false;
                r.Response = "EXCEPTION CAUGHT: " + ex.ToString();
            }
            
            return r;
        }

        public NntpResponse GetArticle(int articleNumber)
        {
            NntpResponse r = new();

            try
            {
                SendCommand($"ARTICLE {articleNumber}");
                ReadResponse(multiline: true);
                r.Success = true;
                r.MultilineResponse = ReadResponse(multiline: true);
            }
            catch (Exception ex)
            {
                r.Success = false;
                r.Response = "EXCEPTION CAUGHT: " + ex.ToString();
            }

            return r;
        }

        #endregion
    }
}
