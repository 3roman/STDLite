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
            byte[] datas = { 0 };
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

        private void AddListItems(IEnumerable<object> items)
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

        private void Form1_Load(object sender, EventArgs e)
        {
            lstMain.ListViewItemSorter = new Common.ListViewColumnSorter();
            lstMain.ColumnClick += new ColumnClickEventHandler(Common.ListViewHelper.ListView_ColumnClick);
        }

    }

}


namespace Common
{
    /// <summary>
    /// 对ListView点击列标题自动排序功能
    /// </summary>
    public class ListViewHelper
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ListViewHelper()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        public static void ListView_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
        {
            System.Windows.Forms.ListView lv = sender as System.Windows.Forms.ListView;
            // 检查点击的列是不是现在的排序列.
            if (e.Column == (lv.ListViewItemSorter as ListViewColumnSorter).SortColumn)
            {
                // 重新设置此列的排序方法.
                if ((lv.ListViewItemSorter as ListViewColumnSorter).Order == System.Windows.Forms.SortOrder.Ascending)
                {
                    (lv.ListViewItemSorter as ListViewColumnSorter).Order = System.Windows.Forms.SortOrder.Descending;
                }
                else
                {
                    (lv.ListViewItemSorter as ListViewColumnSorter).Order = System.Windows.Forms.SortOrder.Ascending;
                }
            }
            else
            {
                // 设置排序列，默认为正向排序
                (lv.ListViewItemSorter as ListViewColumnSorter).SortColumn = e.Column;
                (lv.ListViewItemSorter as ListViewColumnSorter).Order = System.Windows.Forms.SortOrder.Ascending;
            }
            // 用新的排序方法对ListView排序
            ((System.Windows.Forms.ListView)sender).Sort();
        }
    }
    /// <summary>
    /// 继承自IComparer
    /// </summary>
    public class ListViewColumnSorter : System.Collections.IComparer
    {
        /// <summary>
        /// 指定按照哪个列排序
        /// </summary>
        private int ColumnToSort;
        /// <summary>
        /// 指定排序的方式
        /// </summary>
        private System.Windows.Forms.SortOrder OrderOfSort;
        /// <summary>
        /// 声明CaseInsensitiveComparer类对象
        /// </summary>
        private System.Collections.CaseInsensitiveComparer ObjectCompare;
        /// <summary>
        /// 构造函数
        /// </summary>
        public ListViewColumnSorter()
        {
            // 默认按第一列排序
            ColumnToSort = 0;
            // 排序方式为不排序
            OrderOfSort = System.Windows.Forms.SortOrder.None;
            // 初始化CaseInsensitiveComparer类对象
            ObjectCompare = new System.Collections.CaseInsensitiveComparer();
        }
        /// <summary>
        /// 重写IComparer接口.
        /// </summary>
        /// <param name="x">要比较的第一个对象</param>
        /// <param name="y">要比较的第二个对象</param>
        /// <returns>比较的结果.如果相等返回0，如果x大于y返回1，如果x小于y返回-1</returns>
        public int Compare(object x, object y)
        {
            int compareResult;
            System.Windows.Forms.ListViewItem listviewX, listviewY;
            // 将比较对象转换为ListViewItem对象
            listviewX = (System.Windows.Forms.ListViewItem)x;
            listviewY = (System.Windows.Forms.ListViewItem)y;
            string xText = listviewX.SubItems[ColumnToSort].Text;
            string yText = listviewY.SubItems[ColumnToSort].Text;
            int xInt, yInt;
            // 比较,如果值为IP地址，则根据IP地址的规则排序。
            if (IsIP(xText) && IsIP(yText))
            {
                compareResult = CompareIp(xText, yText);
            }
            else if (int.TryParse(xText, out xInt) && int.TryParse(yText, out yInt)) //是否全为数字
            {
                //比较数字
                compareResult = CompareInt(xInt, yInt);
            }
            else
            {
                //比较对象
                compareResult = ObjectCompare.Compare(xText, yText);
            }
            // 根据上面的比较结果返回正确的比较结果
            if (OrderOfSort == System.Windows.Forms.SortOrder.Ascending)
            {
                // 因为是正序排序，所以直接返回结果
                return compareResult;
            }
            else if (OrderOfSort == System.Windows.Forms.SortOrder.Descending)
            {
                // 如果是反序排序，所以要取负值再返回
                return (-compareResult);
            }
            else
            {
                // 如果相等返回0
                return 0;
            }
        }
        /// <summary>
        /// 判断是否为正确的IP地址，IP范围（0.0.0.0～255.255.255）
        /// </summary>
        /// <param name="ip">需验证的IP地址</param>
        /// <returns></returns>
        public bool IsIP(String ip)
        {
            return System.Text.RegularExpressions.Regex.Match(ip, @"^(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])$").Success;
        }
        /// <summary>
        /// 比较两个数字的大小
        /// </summary>
        /// <param name="ipx">要比较的第一个对象</param>
        /// <param name="ipy">要比较的第二个对象</param>
        /// <returns>比较的结果.如果相等返回0，如果x大于y返回1，如果x小于y返回-1</returns>
        private int CompareInt(int x, int y)
        {
            if (x > y)
            {
                return 1;
            }
            else if (x < y)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 比较两个IP地址的大小
        /// </summary>
        /// <param name="ipx">要比较的第一个对象</param>
        /// <param name="ipy">要比较的第二个对象</param>
        /// <returns>比较的结果.如果相等返回0，如果x大于y返回1，如果x小于y返回-1</returns>
        private int CompareIp(string ipx, string ipy)
        {
            string[] ipxs = ipx.Split('.');
            string[] ipys = ipy.Split('.');
            for (int i = 0; i < 4; i++)
            {
                if (Convert.ToInt32(ipxs[i]) > Convert.ToInt32(ipys[i]))
                {
                    return 1;
                }
                else if (Convert.ToInt32(ipxs[i]) < Convert.ToInt32(ipys[i]))
                {
                    return -1;
                }
                else
                {
                    continue;
                }
            }
            return 0;
        }
        /// <summary>
        /// 获取或设置按照哪一列排序.
        /// </summary>
        public int SortColumn
        {
            set
            {
                ColumnToSort = value;
            }
            get
            {
                return ColumnToSort;
            }
        }
        /// <summary>
        /// 获取或设置排序方式.
        /// </summary>
        public System.Windows.Forms.SortOrder Order
        {
            set
            {
                OrderOfSort = value;
            }
            get
            {
                return OrderOfSort;
            }
        }
    }
}
