using System.Windows.Forms;

namespace STDLite.Class
{
    static class Common
    {
        public static void SetClipboard(string context)
        {
            Clipboard.Clear();
            Clipboard.SetData(DataFormats.Text, context);
        }
    }
}

