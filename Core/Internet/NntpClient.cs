using Microsoft.IdentityModel.Tokens;
using NLog;
using System.Net.Sockets;
using System.Text;

namespace Core.Internet
{
    /// <summary>
    /// Response codes: http://www.tcpipguide.com/free/t_NNTPStatusResponsesandResponseCodes-3.htm
    /// </summary>
    public class NntpClient
    {
        #region Fields 

        private const int TIMEOUT = 5000;       // In milliseconds
        private TcpClient? tcpClient;
        private StreamReader? reader;
        private StreamWriter? writer;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Ctors/Dtors 

        public NntpClient()
        {
        }

        #endregion

        #region Generic methods

        private void SendCommand(string command)
        {
            if (writer == null) throw new DisconnectedException();

            Logger.Debug($"CLIENT: {command}");
            writer.WriteLine(command);
        }

        private List<string> ReadResponse(bool multiline = false)
        {
            if (reader == null) throw new DisconnectedException();
            List<string> response = [];
            string? line;

            do
            {
                Logger.Debug("CLIENT *WAITING FOR RESPONSE*");

                line = reader.ReadLine();

                if (line != null)
                {
                    response.Add(line);

                    Logger.Debug("SERVER: " + line);
                }

            } while (line != null && multiline && line.Trim() != ".");

            return response;
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
                    r.Exception = "Connection timeout";
                    return r;
                }

                NetworkStream stream = tcpClient.GetStream();
                stream.ReadTimeout = TIMEOUT;

                reader = new StreamReader(stream, Encoding.ASCII);
                writer = new StreamWriter(stream, Encoding.ASCII) { AutoFlush = true };

                r.RawResponse = reader.ReadLine();
                r.Success = true;
                
                Logger.Debug("SERVER: {0}", r.RawResponse);
            }
            catch (Exception ex)
            {
                r.Success = false;
                r.Exception = ex.ToString();
            }

            return r;
        }

        public NntpQuitResponse Quit()
        {
            var qr = new NntpQuitResponse();

            try
            {
                SendCommand("QUIT");
                Logger.Debug("CLIENT *WAITING FOR RESPONSE*");

                if (reader == null)
                {
                    qr.Success = false;
                    qr.Exception = "Null reader.";
                }
                else
                {
                    qr.RawResponse = reader.ReadLine();
                    Logger.Debug("SERVER: {0}", qr.RawResponse);
                }
                
                tcpClient?.Close();
                
                qr.Success = true;
            }
            catch(Exception ex)
            {
                qr.Success = false;
                qr.Exception = ex.ToString();
            }
            
            return qr;
        }

        public NntpListResponse List()
        {
            var lr = new NntpListResponse();

            try
            {
                SendCommand("LIST");
                Logger.Debug("CLIENT *WAITING FOR RESPONSE*");
                
                if (reader == null) throw new DisconnectedException();
                string? line;
                int line_num = 0;

                do
                {
                    Logger.Debug("CLIENT *WAITING FOR RESPONSE*");

                    line = reader.ReadLine();
                    line_num++;

                    if (line != null) 
                    {
                        Logger.Debug("SERVER: " + line);
                        lr.RawResponse += line + Environment.NewLine;
                        
                        if (line == ".")
                        {
                            break;
                        }

                        if (line_num == 1)
                        {
                            if (lr.ResponseCode != NntpResponseCode.ListFollows)
                            {
                                break;
                            }
                        }
                        else
                        {
                            string[] parts = line.Split(' ');
                            string name = parts[0];
                            int high = Convert.ToInt32(parts[1]);
                            int low = Convert.ToInt32(parts[2]);
                            bool? ok = null;
                            if (parts.Length > 3)
                            {
                                if (parts[3].Equals("y",
                                    StringComparison.CurrentCultureIgnoreCase))
                                    ok = true;
                                else
                                    ok = false;
                            }

                            Logger.Debug("CLIENT *ADDING ITEM*");
                            lr.Items.Add(new NntpListResponseItem(name, low, high, ok));
                        }
                    }

                } while (line != null && line.Trim() != ".");

                lr.Success = true;
            }
            catch (Exception ex)
            {
                lr.Success = false;
                lr.Exception = ex.ToString();
            }

            return lr;
        }

        public NntpGroupResponse Group(string group)
        {
            var gr = new NntpGroupResponse();

            try
            {
                SendCommand($"GROUP {group}");
                Logger.Debug("CLIENT *WAITING FOR RESPONSE*");

                if (reader == null) throw new DisconnectedException();
                string? line = reader.ReadLine();
                gr.RawResponse = line;
                Logger.Debug("SERVER: {0}", gr.RawResponse);

                if (line == null)
                {
                    gr.Success = false;
                    gr.Exception = "Null line";
                }
                else
                {
                    string[] parts = line.Split(" ");

                    if (parts.Length != 5 || gr.ResponseCode != NntpResponseCode.GroupSelected)
                    {
                        gr.Success = false;
                        gr.Exception = "Invalid response";
                    }
                    else
                    {
                        gr.ArticleCount = Convert.ToInt32(parts[1]);
                        gr.FirstArticle = Convert.ToInt32(parts[2]);
                        gr.LastArticle = Convert.ToInt32(parts[3]);
                        gr.Success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                gr.Success = false;
                gr.Exception = ex.ToString();
            }

            return gr;
        }

        public NntpArticleResponse Article()
        {
            var ar = new NntpArticleResponse();

            try
            {
                SendCommand("ARTICLE");
                Logger.Debug("CLIENT *WAITING FOR RESPONSE*");

                if (reader == null) throw new DisconnectedException();
                string? line;
                int line_num = 0;

                do
                {
                    Logger.Debug("CLIENT *WAITING FOR RESPONSE*");

                    line = reader.ReadLine();
                    line_num++;

                    if (line != null)
                    {
                        Logger.Debug("SERVER: " + line);
                        ar.RawResponse += line + Environment.NewLine;

                        if (line == ".")
                        {
                            break;
                        }

                        if (line_num == 1)
                        {
                            if (ar.ResponseCode != NntpResponseCode.ArticleFound)
                            {
                                break;
                            }
                            else
                            {
                                //220 188 <105t835$9cn$1@newsfeed.man.lodz.pl> article

                                string[] parts = line.Split(' ');
                                ar.ArticleNumber = Convert.ToInt32(parts[1]);
                                ar.MessageId = parts[2];
                            }
                        }
                        else
                        {
                            if (line.StartsWith("Subject: ") && ar.Subject == null)
                            {
                                ar.Subject = line;
                            }
                            else
                            {
                                ar.Body += line + Environment.NewLine;
                            }
                        }
                    }

                } while (line != null && line.Trim() != ".");

                ar.Success = true;
            }
            catch (Exception ex)
            {
                ar.Success = false;
                ar.Exception = ex.ToString();
            }

            return ar;
        }

        public NntpNextResponse Next()
        {
            var nr = new NntpNextResponse();

            try
            {
                SendCommand($"NEXT");
                Logger.Debug("CLIENT *WAITING FOR RESPONSE*");

                if (reader == null) throw new DisconnectedException();
                string? line = reader.ReadLine();
                nr.RawResponse = line;
                Logger.Debug("SERVER: {0}", nr.RawResponse);

                if (line == null)
                {
                    nr.Success = false;
                    nr.Exception = "Null line";
                }
                else
                {
                    if (nr.ResponseCode != NntpResponseCode.ArticleFound)
                    {
                        nr.Success = false;
                        nr.Exception = "Invalid response";
                    }
                    else
                    {
                        string[] parts = line.Split(' ');
                        nr.ArticleNumber = Convert.ToInt32(parts[1]);
                        nr.MessageId = parts[2];
                        nr.Success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                nr.Success = false;
                nr.Exception = ex.ToString();
            }

            return nr;
        }

        #endregion
    }
}


