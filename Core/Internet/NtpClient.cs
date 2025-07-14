using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Core.Internet
{
    public class NtpClient
    {
        public static DateTime Query(string ntpServer = "pool.ntp.org")
        {
            byte[] ntpData = new byte[48];
            ntpData[0] = 0x1B; // LI = 0, VN = 3, Mode = 3

            IPAddress[] addresses = Dns.GetHostAddresses(ntpServer);
            if (addresses.Length == 0)
                throw new AddressResolutionException();
                
            IPEndPoint ipEndPoint = new(addresses[0], 123);

            using UdpClient udpClient = new();
            udpClient.Connect(ipEndPoint);
            udpClient.Send(ntpData, ntpData.Length);

            var asyncResult = udpClient.BeginReceive(null, null);
            asyncResult.AsyncWaitHandle.WaitOne(3000); // Wait max 3 sec

            if (!asyncResult.IsCompleted)
                throw new RequestTimeoutException();

            IPEndPoint? remoteEndPoint = null;
            byte[] receivedData = udpClient.EndReceive(asyncResult, ref remoteEndPoint);

            if (receivedData.Length < 48)
                throw new IncompleteResponseException();

            ulong intPart = BitConverter.ToUInt32(receivedData, 40);
            ulong fractPart = BitConverter.ToUInt32(receivedData, 44);

            intPart = NetworkHelper.SwapEndianness(intPart);
            fractPart = NetworkHelper.SwapEndianness(fractPart);

            ulong milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);
            DateTime networkDateTime = new DateTime(1900, 1, 1).AddMilliseconds((long)milliseconds);

            return networkDateTime.ToLocalTime();
        }
    }
}
