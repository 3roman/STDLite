using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Windows.Forms;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace SEG_SD
{
    public partial class Form1 : Form
    {
        Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var dllName = args.Name.Contains(",") ? args.Name.Substring(0, args.Name.IndexOf(',')) : args.Name.Replace(".dll", "");
            dllName = dllName.Replace(".", "_");
            if (dllName.EndsWith("_resources")) return null;
            var rsc = new ResourceManager(GetType().Namespace + ".Properties.Resources",
                Assembly.GetExecutingAssembly());
            var buffer = (byte[])rsc.GetObject(dllName);
            return Assembly.Load(buffer);
        }

        public void SetFormTitle()
        {
            const string title = "{0}  V{1}   c0de by hangch";
            Text = string.Format(title, Application.ProductName, Application.ProductVersion);
        }

        public string RetrieveHtml(string url, string data = "", bool get = true)
        {
            var wc = new WebClient();
            wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            var postData = Encoding.UTF8.GetBytes(data);
            byte[] datas = {0};
            try
            {
                datas = get ? wc.DownloadData(url) : wc.UploadData(url, "POST", postData);
            }
            catch (Exception ex)
            {
                SetFormTitle();
                MessageBox.Show(ex.Message);
                throw;
            }

            return Encoding.UTF8.GetString(datas);
        }

        private static void DownloadStandard(string filePath, string url)
        {
            var wc = new WebClient();
            var datas = wc.DownloadData(url);
            var fs = new FileStream(filePath, FileMode.Create);
            fs.Write(datas, 0, datas.Length);
            fs.Close();

        }

        public void GetStandard(string url, ref List<object> items)
        {
            var postData = @"__EVENTTARGET=stdlist%24lbtn_refresh_1&__VIEWSTATE=%2FwEPDwULLTE2NjMyMjA0OTYPZBYCAgEPZBYEZg8PZBYCHgVzdHlsZQUkbWFyZ2luOjBweDtwYWRkaW5nOjBweDtkaXNwbGF5Om5vbmU7ZAIBDw9kFgIfAAUkZGlzcGxheTpub25lO21hcmdpbjowcHg7cGFkZGluZzowcHg7ZBgBBR5fX0NvbnRyb2xzUmVxdWlyZVBvc3RCYWNrS2V5X18WCwUdc3RkbGlzdCRTb3J0RmllbGRfUGVyZm9ybWRhdGUFHXN0ZGxpc3QkU29ydEZpZWxkX1B1Ymxpc2hkYXRlBR1zdGRsaXN0JFNvcnRGaWVsZF9QdWJsaXNoZGF0ZQUdc3RkbGlzdCRTb3J0RmllbGRfRXhwaXJlZGRhdGUFHXN0ZGxpc3QkU29ydEZpZWxkX0V4cGlyZWRkYXRlBRxzdGRsaXN0JFNvcnRGaWVsZF9QZXJtaXRkZXB0BRxzdGRsaXN0JFNvcnRGaWVsZF9QZXJtaXRkZXB0BRlzdGRsaXN0JFNvcnRGaWVsZF9TdGRjb2RlBRlzdGRsaXN0JFNvcnRGaWVsZF9TdGRjb2RlBRtzdGRsaXN0JFNvZnRGaWVsZF9WaWV3Q291bnQFG3N0ZGxpc3QkU29mdEZpZWxkX1ZpZXdDb3VudO8M73tN9IyZEmIzbo3Rt9RG6tux&__EVENTVALIDATION=%2FwEWKwLO8JzAAwKHpeXgAQL7pL24BAKLtoHNCAKH9772BgKR9%2FPABALA9NmOBwLgoYrZCQLA9N2OBwK9s4GIDgLI%2BY3QAgKX%2BOKqAgLh%2B4PaDQKdsbKgDgKi%2F46dBALzwuvZBgLJ2OjZBgKKy%2BGqDQLqycYZAv7lkOIDAurJyjkC%2FuWUggQC6sme8AEC%2FuW4uQUC6smikAIC%2FuW82QUC6smWsAEC%2FuWg%2BQQC6sma0AEC%2FuWkmQUC6smu6QIC%2FuWIsgYC6smyiQMC%2FuWM0gYC6smmsAIC%2FuWw%2BQUC6smq0AIC%2FuW0mQYCmPePxAoCwPThjgcC96GGwgYCwPTFjgcC5trLyAEB9COtPt6LDYSCzyGfSC34U4IzoA%3D%3D&stdlist%24txt_page_count_top=1000000";
            var html = RetrieveHtml(url, postData, false);
            
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            const string xPath = "//a[@href]";
            var nodes = doc.DocumentNode.SelectNodes(xPath);
            if (null == nodes) return;

            foreach (var node in nodes)
            {
                if (!node.InnerText.Contains("---")) continue;
                var innerText = node.InnerText.Replace("---", "^").Split('^');
                items.Add(new
                {
                    number = innerText[1].Replace("/", ""),
                    name = innerText[0],
                    link = "http://10.113.1.69/std/showEBook.aspx?fileid=" + node.Attributes["href"].Value.Replace("../std/stdmgr.aspx?fileid=", "")
                });
            }

            items = items.Distinct().ToList();
        }

        private void AddListItems(IEnumerable<object> items )
        {
            lstMain.BeginUpdate();
            lstMain.Items.Clear();
            
            foreach (var item in items)
            {
                // 通过反射获取匿名对象属性
                var lvi = new ListViewItem(lstMain.Items.Count.ToString());
                lvi.SubItems.Add(item.GetType().GetProperty("number").GetValue(item, null).ToString());
                lvi.SubItems.Add(item.GetType().GetProperty("name").GetValue(item, null).ToString());
                lvi.SubItems.Add(item.GetType().GetProperty("link").GetValue(item, null).ToString());
                lstMain.Items.Add(lvi);
            }
            lstMain.EndUpdate();

        }
       
        public Form1()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            InitializeComponent();
            SetFormTitle();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var items = new List<object>();

            Text = "努力检索中...马上就来";
            var url = string.Format("http://10.113.1.69/std/stdsearch.aspx?key={0}&idx=0&pcnt=10000",
                txtKeyword.Text);
            GetStandard(url, ref items);
            url = string.Format("http://10.113.1.69/std/stdsearch.aspx?key={0}&idx=1&pcnt=10000",
                txtKeyword.Text);
            GetStandard(url, ref items);
            AddListItems(items);

            SetFormTitle();

        }

        private void lstMain_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            Text = "玩命下载中...稍安勿躁(*@ο@*)";
            //var filePath = string.Format("{0}\\{1} {2}.pdf",
            //    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            //    lstMain.SelectedItems[0].SubItems[1].Text,
            //    lstMain.SelectedItems[0].SubItems[2].Text);
            var filePath = string.Format("{0} {1}.pdf",
                lstMain.SelectedItems[0].SubItems[1].Text,
                lstMain.SelectedItems[0].SubItems[2].Text);
            var uri = lstMain.SelectedItems[0].SubItems[3].Text;
            DownloadStandard(filePath, uri);
            Cursor = Cursors.Default;
            Process.Start(filePath);
            SetFormTitle();

        }

        private void txtKeyword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)
                txtKeyword.Clear();
        }

    }
}
