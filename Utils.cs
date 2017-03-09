using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace STDLite
{
    static class Utils
    {
        public static void SetClipboard(string context)
        {
            Clipboard.Clear();
            Clipboard.SetData(DataFormats.Text, context);
        }

        public static bool CheckPortAlive(string host, int port, int secondsTimeout)
        {
            if (!IPAddress.TryParse(host, out IPAddress ip))
            {
                ip = Dns.GetHostAddresses(host)[0];
            }

            var connectedResult = new TcpClient().BeginConnect(ip, port, null, null);
            connectedResult.AsyncWaitHandle.WaitOne(secondsTimeout * 1000, true);

            if (connectedResult.IsCompleted)
            {
                return true;
            }

            return false;
        }

        public static bool HasChinese(string content)
        {
            return Regex.IsMatch(content, @"[\u4e00-\u9fa5]") ? true : false;
        }

    }


}

