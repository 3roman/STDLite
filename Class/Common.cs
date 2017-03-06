using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace STDLite
{
    static class Common
    {
        public static void SetClipboard(string context)
        {
            Clipboard.Clear();
            Clipboard.SetData(DataFormats.Text, context);
        }

        public static bool CheckPortAlive(string host, int port, int secondsTimeout)
        {
            var isPortAlive = false;

            IPAddress ip;
            if (!IPAddress.TryParse(host, out ip))
            {
                ip = Dns.GetHostAddresses(host)[0];
            }

            var connectResult = new TcpClient().BeginConnect(ip, port, null, null);
            connectResult.AsyncWaitHandle.WaitOne(secondsTimeout * 1000, true);

            if (connectResult.IsCompleted)
            {
                isPortAlive = true;
            }

            return isPortAlive;
        }

    }


}

