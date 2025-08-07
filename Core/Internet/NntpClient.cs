using Microsoft.IdentityModel.Logging;
using System.Net.Sockets;
using System.Reflection.PortableExecutable;
using System.Text;

namespace Core.Internet
{
    public class NntpClient
    {
        #region Fields 

        private const int TIMEOUT = 5000;       // In milliseconds
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

        #region Generic methods

        private void SendCommand(string command)
        {
            if (writer == null) throw new DisconnectedException();

            if (_verbose)
                Console.WriteLine("CLIENT: " + command);

            writer.WriteLine(command);
        }

        private List<string> ReadResponse(bool multiline = false)
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

        #endregion

        #region Commands

        public NntpConnectResponse Connect(string server, int port = 119)
        {
            NntpConnectResponse r = new();

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

        public NntpListResponse GetNewsgroups()
        {
            NntpListResponse r = new();

            try
            {
                SendCommand("LIST");
                List<string> response = ReadResponse(multiline: true);
                r.Success = true;
                r.MultilineResponse = Filter(response, ["215", "."]);

                foreach(string item in r.MultilineResponse)
                {
                    string[] parts = item.Split(' ');
                    string name = parts[0];
                    int high = Convert.ToInt32(parts[1]);
                    int low = Convert.ToInt32(parts[2]);
                    bool ok = false;
                    if (parts[3].Equals("y", 
                        StringComparison.CurrentCultureIgnoreCase))
                        ok = true;

                    r.Items.Add(new NntpListResponseItem(name, low, high, ok));
                }
            }
            catch(Exception ex)
            {
                r.Success = false;
                r.Response += "EXCEPTION CAUGHT: " + ex.ToString();
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

        public NntpGroupResponse SelectNewsgroup(string group)
        {
            NntpGroupResponse r = new();

            try
            {
                SendCommand($"GROUP {group}");
                List<string> response = ReadResponse();
                r.Success = true;

                if (response.Count == 1)
                {
                    r.Response = response[0];

                    string[] parts = r.Response.Split(" ");

                    if (parts.Length != 5 || r.Response.StartsWith("411"))
                    {
                        r.Success = false;
                    }
                    else
                    {
                        r.ArticleCount = Convert.ToInt32(parts[1]);
                        r.FirstArticle = Convert.ToInt32(parts[2]);
                        r.LastArticle = Convert.ToInt32(parts[3]);
                    }
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

        public NntpArticleResponse GetArticle()
        {
            NntpArticleResponse r = new();

            try
            {
                SendCommand("ARTICLE");
                ReadResponse(multiline: true);
                r.Success = true;
                r.MultilineResponse = ReadResponse(multiline: true);

                foreach (string line in r.MultilineResponse)
                {
                    if (line.StartsWith("Subject: "))
                    {
                        r.Header = line[9..];
                    }
                    else
                    {
                        r.Body += line + Environment.NewLine;
                    }
                }
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
