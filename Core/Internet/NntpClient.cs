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
                ReadResponse(true);
                //if (reader == null)
                //{
                //    lr.Success = false;
                //    lr.Exception = "Null reader.";
                //}
                //else
                //{
                //    string? line;

                //    do
                //    {
                //        Logger.Debug("CLIENT *WAITING FOR RESPONSE*");
                //        line = reader.ReadLine();
                //        bool first = true;

                //        if (line != null)
                //        {
                //            Logger.Debug("SERVER: " + line);
                //            lr.RawResponse += line + Environment.NewLine;

                //            if (first)
                //            {
                //                // First line may have a response code. 
                //                // Depending on the response code, this may be the only line.
                //                Logger.Debug("CLIENT *FIRST RESPONSE LINE*");

                //                int numeric_code = Convert.ToInt32(line[..3]);
                //                NntpResponseCode response_code = (NntpResponseCode)numeric_code;
                //                if (response_code != NntpResponseCode.ListFollows)
                //                {
                //                    lr.Success = false;
                //                    lr.Exception = "Invalid list response code: " + numeric_code;
                //                    break;
                //                }
                                
                //                first = false;
                //            }
                //            else
                //            {

                //                string[] parts = line.Split(' ');
                //                string name = parts[0];
                //                int high = Convert.ToInt32(parts[1]);
                //                int low = Convert.ToInt32(parts[2]);
                //                bool? ok = null;
                //                if (parts.Length > 3)
                //                {
                //                    if (parts[3].Equals("y",
                //                        StringComparison.CurrentCultureIgnoreCase))
                //                        ok = true;
                //                    else
                //                        ok = false;
                //                }

                //                Logger.Debug("CLIENT *ADDING ITEM*");
                //                lr.Items.Add(new NntpListResponseItem(name, low, high, ok));
                //            }
                //        }
                //    } while (line != null && line.Trim() != ".");
                //}
            }
            catch (Exception ex)
            {
                lr.Success = false;
                lr.Exception = ex.ToString();
            }

            return lr;
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

        //public NntpGroupResponse Group(string group)
        //{
        //    NntpGroupResponse r = new();

        //    try
        //    {
        //        SendCommand($"GROUP {group}");
        //        List<string> response = ReadResponse();
        //        r.Success = true;

        //        if (response.Count == 1)
        //        {
        //            r.Response = response[0];

        //            string[] parts = r.Response.Split(" ");

        //            if (parts.Length != 5 || r.Response.StartsWith("411"))
        //            {
        //                r.Success = false;
        //            }
        //            else
        //            {
        //                r.ArticleCount = Convert.ToInt32(parts[1]);
        //                r.FirstArticle = Convert.ToInt32(parts[2]);
        //                r.LastArticle = Convert.ToInt32(parts[3]);
        //            }
        //        }
        //        else
        //        {
        //            r.Success = false;
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        r.Success = false;
        //        r.Response = "EXCEPTION CAUGHT: " + ex.ToString();
        //        r.Exception = ex.ToString();
        //    }

        //    return r;
        //}

        //public NntpArticleResponse Article()
        //{
        //    NntpArticleResponse r = new();

        //    try
        //    {
        //        SendCommand("ARTICLE");
        //        List<string> response = ReadResponse(multiline: true);
        //        r.Success = true;
        //        r.MultilineResponse = Filter(response, ["."]);

        //        string[] parts = r.MultilineResponse[0].Split(" ");
        //        r.ResponseCode = Convert.ToInt32(parts[0]);

        //        if (r.ResponseCode == 420)
        //        {
        //            r.Success = false;
        //            return r;
        //        }

        //        if (parts.Length > 1)
        //        {
        //            r.ArticleNumber = Convert.ToInt32(parts[1]);
        //            r.MessageId = parts[2];
        //        }

        //        for(int i = 1; i < r.MultilineResponse.Count; i++)
        //        {
        //            string line = r.MultilineResponse[i];
        //            if (line.StartsWith("Subject: "))
        //                r.Header = line[9..];
        //            else
        //                r.Body += line + Environment.NewLine;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        r.Success = false;
        //        r.Response = "EXCEPTION CAUGHT: " + ex.ToString();
        //        r.Exception = ex.ToString();
        //    }

        //    return r;
        //}

        //public NntpNextResponse Next()
        //{
        //    NntpNextResponse r = new();

        //    try
        //    {
        //        SendCommand($"NEXT");
        //        List<string> response = ReadResponse(false);
        //        r.Success = true;

        //        if (response.Count == 1)
        //        {
        //            r.Response = response[0];
        //        }
        //        else
        //        {
        //            r.Success = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        r.Success = false;
        //        r.Response = "EXCEPTION CAUGHT: " + ex.ToString();
        //        r.Exception = ex.ToString();
        //    }

        //    return r;
        //}

        #endregion
    }
}


