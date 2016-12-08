using System;
using System.Reflection;
using System.Windows.Forms;
using STDLite.Class;


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
            //////////////////////////////////////////////////////////////////////////////////////
            // 加载嵌入dll资源到内存
            EmbeddedAssembly.Load("STDLite.Resource.HtmlAgilityPack.dll", "HtmlAgilityPack.dll");
            AppDomain.CurrentDomain.AssemblyResolve += (sender, senderArgs) => EmbeddedAssembly.Get(senderArgs.Name);
            //////////////////////////////////////////////////////////////////////////////////////
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMain());

        }
    }
}
