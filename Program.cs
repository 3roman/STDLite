using System;
using System.Windows.Forms;
using System.Threading;

namespace STDLite
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            // 
            new Thread(() => Utils.ScanPort("10.113.1.69", 80))
             {
                 IsBackground = true
             }.Start();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMain());

        }
    }
}
