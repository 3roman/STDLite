using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.Win32;
using htd = HtmlAgilityPack.HtmlDocument;

namespace STDLite
{


    public partial class FormMain : Form
    {
        public FormMain()
        {
            AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;

            InitializeComponent();
            ResetUI();

            lstMain.ListViewItemSorter = new ListViewColumnSorter();
            lstMain.ColumnClick += ListViewHelper.ListView_ColumnClick;

            var value = RegistryHelper.ReadValue(null, "Software\\STDLite", "OnTop");
            TopMost = "True" == value;
            mnuOnTop.Checked = "True" == value;

        }

        private Assembly ResolveAssembly(object sender, ResolveEventArgs args)
        {
            var dllName = args.Name.Contains(",")
                ? args.Name.Substring(0, args.Name.IndexOf(','))
                : args.Name.Replace(".dll", "");
            dllName = dllName.Replace(".", "_");
            if (dllName.EndsWith("_resources")) return null;
            var rsc = new ResourceManager(GetType().Namespace + ".Properties.Resources",
                Assembly.GetExecutingAssembly());
            var buffer = (byte[])rsc.GetObject(dllName);

            return Assembly.Load(buffer);
        }

        // ReSharper disable once InconsistentNaming
        private void ResetUI()
        {
            Text = string.Format("{0}  V{1}   工艺系统室专版",
                Application.ProductName,
                Application.ProductVersion);
            Cursor = Cursors.Default;
        }

        private static void Copy2Clipboard(string content)
        {
            Clipboard.Clear();
            Clipboard.SetData(DataFormats.Text, content);
        }

        private static long GetContentLength(string url)
        {
            try
            {
                var req = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
                req.Method = "HEAD";
                req.Timeout = 2000;
                var res = (HttpWebResponse)req.GetResponse();
                var length = 0L;
                if (HttpStatusCode.OK == res.StatusCode)
                {
                    length = res.ContentLength;
                }

                res.Close();

                return length;
            }
            catch (WebException)
            {
                return 0;
            }
        }

        private static void GetStandards(string url, string data, ref List<ListElement> elements)
        {
            var wc = new WebClient { Encoding = Encoding.UTF8 };
            //下面一句很关键，不然返回的东西不全
            wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            var html = wc.UploadData(new Uri(url), "POST", Encoding.UTF8.GetBytes(data));

            var doc = new htd();
            doc.LoadHtml(Encoding.UTF8.GetString(html));

            //const string xPath = "//a[@href]";
            const string xPath = "//a[contains(@href,'/std/stdmgr.aspx')]";
            var nodes = doc.DocumentNode.SelectNodes(xPath);
            if (null == nodes)
            {
                return;
            }

            elements.AddRange(from node in nodes
                              let innerTexts = node.InnerText.Replace("---", "^").Split('^')
                              select new ListElement
                              {
                                  // 标准号
                                  Number = innerTexts[1].ToUpper(),
                                  // 标准名
                                  Name = innerTexts[0],
                                  // 下载地址(由特定URL与id组成)
                                  Link = "http://10.113.1.69/std/showEBook.aspx?fileid=" + node.Attributes["href"].Value.Replace("../std/stdmgr.aspx?fileid=", ""),
                                  // 是否过期
                                  Validate = node.ParentNode.ParentNode.ChildNodes[13].ChildNodes[1].InnerText.Replace("标准状态:", "")
                              });
        }

        private void DownloadFile(string uri, string filename)
        {
            var wc = new WebClient();
            wc.DownloadProgressChanged += OnDownloadProgressChanged;
            // 匿名委托使用外部变量，传递文件保存路径
            wc.DownloadFileCompleted += (s, e) => OnDownloadFileCompleted(filename);
            wc.DownloadFileAsync(new Uri(uri), filename);
        }

        private void OnDownloadFileCompleted(string filename)
        {
            var fi = new FileInfo(filename);
            var fileLength = (float)fi.Length / 1024 ;

            if (fileLength > 10)
            {
                Process.Start(filename);
            }
            else
            {
                File.Delete(filename);
                MessageBox.Show("SEG标准网站暂未收录该标准", "报错提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            ResetUI();
        }

        private void OnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            var title = string.Format("下载进度：{0}%  >>>  {1}kB/{2}kB",
                e.ProgressPercentage,
                e.BytesReceived / 1024,
                e.TotalBytesToReceive / 1024
                );
            Text = title;
        }

        private void FillList(IEnumerable<ListElement> elements)
        {
            lstMain.BeginUpdate();
            lstMain.Items.Clear();

            foreach (var e in elements)
            {
                var lvi = new ListViewItem(e.Number);
                lvi.SubItems.Add(e.Name);
                lvi.SubItems.Add(e.Validate);
                lvi.SubItems.Add(e.Link);
                lstMain.Items.Add(lvi);

                switch (e.Validate)
                {
                    case "未实施":
                        lvi.ForeColor = Color.Green;
                        break;
                    case "已过期":
                        lvi.ForeColor = Color.Red;
                        break;
                }
            }

            lstMain.EndUpdate();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (GetContentLength("http://10.113.1.69/std/stdsearch.aspx") < 5000)
            {
                MessageBox.Show("SEG标准网站在维护中...", "报错提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var keyword = txtKeyword.Text;
            if (!keyword.ToLower().Contains("/t"))
            {
                keyword = keyword.ToLower().Replace("t", "/t");
                txtKeyword.Text = keyword;
            }

            Text = "努力检索中......";
            Cursor = Cursors.WaitCursor;
            var elements = new List<ListElement>();
            const string data = "__EVENTTARGET=stdlist%24lbtn_refresh_2&__EVENTARGUMENT=&__VIEWSTATE=%2FwEPDwULLTE2NjMyMjA0OTYPZBYCAgEPZBYEZg8PZBYCHgVzdHlsZQUkbWFyZ2luOjBweDtwYWRkaW5nOjBweDtkaXNwbGF5Om5vbmU7ZAIBDw9kFgIfAAUkZGlzcGxheTpub25lO21hcmdpbjowcHg7cGFkZGluZzowcHg7ZBgBBR5fX0NvbnRyb2xzUmVxdWlyZVBvc3RCYWNrS2V5X18WCwUdc3RkbGlzdCRTb3J0RmllbGRfUGVyZm9ybWRhdGUFHXN0ZGxpc3QkU29ydEZpZWxkX1B1Ymxpc2hkYXRlBR1zdGRsaXN0JFNvcnRGaWVsZF9QdWJsaXNoZGF0ZQUdc3RkbGlzdCRTb3J0RmllbGRfRXhwaXJlZGRhdGUFHXN0ZGxpc3QkU29ydEZpZWxkX0V4cGlyZWRkYXRlBRxzdGRsaXN0JFNvcnRGaWVsZF9QZXJtaXRkZXB0BRxzdGRsaXN0JFNvcnRGaWVsZF9QZXJtaXRkZXB0BRlzdGRsaXN0JFNvcnRGaWVsZF9TdGRjb2RlBRlzdGRsaXN0JFNvcnRGaWVsZF9TdGRjb2RlBRtzdGRsaXN0JFNvZnRGaWVsZF9WaWV3Q291bnQFG3N0ZGxpc3QkU29mdEZpZWxkX1ZpZXdDb3VudO8M73tN9IyZEmIzbo3Rt9RG6tux&__EVENTVALIDATION=%2FwEWKwLO8JzAAwKHpeXgAQL7pL24BAKLtoHNCAKH9772BgKR9%2FPABALA9NmOBwLgoYrZCQLA9N2OBwK9s4GIDgLI%2BY3QAgKX%2BOKqAgLh%2B4PaDQKdsbKgDgKi%2F46dBALzwuvZBgLJ2OjZBgKKy%2BGqDQLqycYZAv7lkOIDAurJyjkC%2FuWUggQC6sme8AEC%2FuW4uQUC6smikAIC%2FuW82QUC6smWsAEC%2FuWg%2BQQC6sma0AEC%2FuWkmQUC6smu6QIC%2FuWIsgYC6smyiQMC%2FuWM0gYC6smmsAIC%2FuWw%2BQUC6smq0AIC%2FuW0mQYCmPePxAoCwPThjgcC96GGwgYCwPTFjgcC5trLyAEB9COtPt6LDYSCzyGfSC34U4IzoA%3D%3D&stdlist%24hd_dir=&stdlist%24hd_key=635983975871817969&stdlist%24txt_page_count_top=100000&stdlist%24txt_page_index_top=1&stdlist%24SortFieldGrp1=SortField_Performdate&stdlist%24cboSortDirection=%E9%99%8D%E5%BA%8F&stdlist%24txt_page_count_btm=10&stdlist%24txt_page_index_btm=1";
            // 搜索标准号
            var url = string.Format("http://10.113.1.69/std/stdsearch.aspx?key={0}&idx=1&pcnt=10", keyword);
            GetStandards(url, data, ref elements);
            // 搜索标准名
            url = string.Format("http://10.113.1.69/std/stdsearch.aspx?key={0}&idx=0&pcnt=10", keyword);
            GetStandards(url, data, ref elements);
            ResetUI();
            FillList(elements);
        }

        private void lstMain_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // 构造保存文件路径
                // 获取注册表中的存放路径
                var value = RegistryHelper.ReadValue(null, "Software\\STDLite", "SavedDirectory");
                var directory = Directory.Exists(value) ? value : Environment.CurrentDirectory;
                var filename = directory + "\\" +
                               lstMain.SelectedItems[0].SubItems[0].Text + " " +
                               lstMain.SelectedItems[0].SubItems[1].Text + ".pdf";
                filename = filename.Replace("/", "");

                if (!File.Exists(filename))
                {
                    Cursor = Cursors.WaitCursor;
                    var uri = lstMain.SelectedItems[0].SubItems[3].Text;
                    DownloadFile(uri, filename);
                }
                else
                {
                    // 文件已下载，直接打开
                    Process.Start(filename);
                }

            }
        }

        private void mnuOnTop_Click(object sender, EventArgs e)
        {
            var value = RegistryHelper.ReadValue(null, "Software\\STDLite", "OnTop");
            if ("True" == value)
            {
                RegistryHelper.SetValue(null, "Software\\STDLite", "OnTop", "False");
                TopMost = false;
            }
            else
            {
                RegistryHelper.SetValue(null, "Software\\STDLite", "OnTop", "True");
                TopMost = true;
            }
        }

        private void munCopySTDId_Click(object sender, EventArgs e)
        {
            Copy2Clipboard(lstMain.SelectedItems[0].SubItems[0].Text);
        }

        private void munCopySTDName_Click(object sender, EventArgs e)
        {
            Copy2Clipboard(lstMain.SelectedItems[0].SubItems[1].Text);
        }

        private void munCopySTD_Click(object sender, EventArgs e)
        {
            var stdId = lstMain.SelectedItems[0].SubItems[0].Text;
            var stdName = lstMain.SelectedItems[0].SubItems[1].Text;
            Copy2Clipboard(stdId + stdName);
        }

        private void munSaveAs_Click(object sender, EventArgs e)
        {
            // 注册表中路径不存在则使用程序所在目录
            var value = RegistryHelper.ReadValue(null, "Software\\STDLite", "SavedDirectory");
            var path = Directory.Exists(value) ? value : Environment.CurrentDirectory;
            // 构造文件名
            var pdfName = lstMain.SelectedItems[0].SubItems[0].Text + " " + lstMain.SelectedItems[0].SubItems[1].Text;
            pdfName = pdfName.Replace("/", "");
            var sfd = new SaveFileDialog
            {
                InitialDirectory = path, // 设置初始目录
                Filter = "PDF文件|*.pdf|所有文件|*.*",
                FileName = pdfName // 设置默认保存文件名
            };

            if (sfd.ShowDialog() != DialogResult.OK) return;
            Cursor = Cursors.WaitCursor;
            var filename = sfd.FileName;
            var uri = lstMain.SelectedItems[0].SubItems[3].Text;
            DownloadFile(uri, filename);

            // 存储此次保存的路径
            var savedDirectory = filename.Substring(0, filename.LastIndexOf("\\", StringComparison.Ordinal));
            RegistryHelper.SetValue(null, "Software\\STDLite", "SavedDirectory", savedDirectory);
        }

        private void munOpenSavedDirectory_Click(object sender, EventArgs e)
        {
            var value = RegistryHelper.ReadValue(null, "Software\\STDLite", "SavedDirectory");
            Process.Start(Directory.Exists(value) ? value : Environment.CurrentDirectory);
        }

        private void mnuCheckUpdate_Click(object sender, EventArgs e)
        {
            Process.Start("http://10.151.130.55/forum.php?mod=viewthread&tid=443&page=1");
        }

        private void mnuAbout_Click(object sender, EventArgs e)
        {

        }

        private void txtKeyword_KeyDown(object sender, KeyEventArgs e)
        {
            if ("关键字示例：【SHT 3405】【发电厂设计规范】【化工 泵】" == txtKeyword.Text)
            {
                txtKeyword.ForeColor = Color.Black;
                txtKeyword.Clear();
            }
            else if (e.KeyData == Keys.Escape)
            {
                txtKeyword.Clear();
            }
        }
    }
}

namespace Common
{
    class ListElement
    {
        public string Name;
        public string Number;
        public string Link;
        public string Validate;

    }

    /// <summary>
    /// 对ListView点击列标题自动排序功能
    /// </summary>
    class ListViewHelper
    {
        public static void ListView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            var lv = sender as ListView;
            // 检查点击的列是不是现在的排序列.
            if (null != lv && e.Column == ((ListViewColumnSorter)lv.ListViewItemSorter).SortColumn)
            {
                // 重新设置此列的排序方法.
                ((ListViewColumnSorter)lv.ListViewItemSorter).Order =
                    ((ListViewColumnSorter)lv.ListViewItemSorter).Order == SortOrder.Ascending
                        ? SortOrder.Descending
                        : SortOrder.Ascending;
            }
            else
            {
                // 设置排序列，默认为正向排序
                if (null != lv)
                {
                    ((ListViewColumnSorter)lv.ListViewItemSorter).SortColumn = e.Column;
                    ((ListViewColumnSorter)lv.ListViewItemSorter).Order = SortOrder.Ascending;
                }
            }
            // 用新的排序方法对ListView排序
            ((ListView)sender).Sort();
        }
    }

    /// <summary>
    ///     继承自IComparer
    /// </summary>
    class ListViewColumnSorter : IComparer
    {
        /// <summary>
        ///     声明CaseInsensitiveComparer类对象
        /// </summary>
        private readonly CaseInsensitiveComparer _objectCompare;

        /// <summary>
        ///     构造函数
        /// </summary>
        public ListViewColumnSorter()
        {
            // 默认按第一列排序
            SortColumn = 0;
            // 排序方式为不排序
            Order = SortOrder.None;
            // 初始化CaseInsensitiveComparer类对象
            _objectCompare = new CaseInsensitiveComparer();
        }

        /// <summary>
        ///     获取或设置按照哪一列排序
        /// </summary>
        public int SortColumn { set; get; }

        /// <summary>
        ///     获取或设置排序方式
        /// </summary>
        public SortOrder Order { set; get; }

        /// <summary>
        ///     重写IComparer接口
        /// </summary>
        /// <param name="ipx">要比较的第一个对象</param>
        /// <param name="ipy">要比较的第二个对象</param>
        /// <returns>比较的结果.如果相等返回0，如果x大于y返回1，如果x小于y返回-1</returns>
        public int Compare(object ipx, object ipy)
        {
            int compareResult;
            // 将比较对象转换为ListViewItem对象
            var listviewX = (ListViewItem)ipx;
            var listviewY = (ListViewItem)ipy;
            var xText = listviewX.SubItems[SortColumn].Text;
            var yText = listviewY.SubItems[SortColumn].Text;
            int xInt, yInt;
            // 比较,如果值为IP地址，则根据IP地址的规则排序。
            if (IsIp(xText) && IsIp(yText))
            {
                compareResult = CompareIp(xText, yText);
            }
            else if (int.TryParse(xText, out xInt) && int.TryParse(yText, out yInt)) //是否全为数字
            {
                // 比较数字
                compareResult = CompareInt(xInt, yInt);
            }
            else
            {
                // 比较对象
                compareResult = _objectCompare.Compare(xText, yText);
            }
            // 根据上面的比较结果返回正确的比较结果
            switch (Order)
            {
                case SortOrder.Ascending:
                    // 因为是正序排序，所以直接返回结果
                    return compareResult;
                case SortOrder.Descending:
                    // 如果是反序排序，所以要取负值再返回
                    return (-compareResult);
            }

            // 如果相等返回0
            return 0;
        }

        /// <summary>
        ///     判断是否为正确的IP地址，IP范围（0.0.0.0～255.255.255）
        /// </summary>
        /// <param name="ip">需验证的IP地址</param>
        /// <returns></returns>
        public bool IsIp(string ip)
        {
            return Regex.Match(ip,
                @"^(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])$")
                .Success;
        }

        /// <summary>
        ///     比较两个数字的大小
        /// </summary>
        /// <param name="ipx">要比较的第一个对象</param>
        /// <param name="ipy">要比较的第二个对象</param>
        /// <returns>比较的结果.如果相等返回0，如果x大于y返回1，如果x小于y返回-1</returns>
        public static int CompareInt(int ipx, int ipy)
        {
            if (ipx > ipy)
            {
                return 1;
            }
            if (ipx < ipy)
            {
                return -1;
            }

            return 0;
        }

        /// <summary>
        ///     比较两个IP地址的大小
        /// </summary>
        /// <param name="ipx">要比较的第一个对象</param>
        /// <param name="ipy">要比较的第二个对象</param>
        /// <returns>比较的结果.如果相等返回0，如果x大于y返回1，如果x小于y返回-1</returns>
        public static int CompareIp(string ipx, string ipy)
        {
            var ipxs = ipx.Split('.');
            var ipys = ipy.Split('.');

            for (var i = 0; i < 4; i++)
            {
                if (Convert.ToInt32(ipxs[i]) > Convert.ToInt32(ipys[i]))
                {
                    return 1;
                }
                if (Convert.ToInt32(ipxs[i]) < Convert.ToInt32(ipys[i]))
                {
                    return -1;
                }
            }

            return 0;
        }
    }

    class RegistryHelper
    {
        public static string ReadValue(RegistryKey rootKey, string subkey, string name)
        {
            var value = string.Empty;
            if (null == rootKey) rootKey = Registry.LocalMachine;
            try
            {
                var subKey = rootKey.OpenSubKey(subkey, true);
                value = subKey.GetValue(name) + "";
                subKey.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return value;
        }

        public static void SetValue(RegistryKey rootKey, string subkey, string name, string value)
        {
            if (null == rootKey) rootKey = Registry.LocalMachine;
            try
            {
                var subKey = rootKey.CreateSubKey(subkey);
                subKey.SetValue(name, value);
                subKey.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void DeleteKey(RegistryKey rootKey, string subkey, string name)
        {
            if (null == rootKey) rootKey = Registry.LocalMachine;
            try
            {
                var subKey = rootKey.OpenSubKey(subkey, true);

                var subKeys = subKey.GetSubKeyNames();
                foreach (var x in subKeys.Where(x => x == name))
                {
                    subKey.DeleteSubKeyTree(name);
                }

                subKey.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public bool IsKeyExist(RegistryKey rootKey, string subkey, string name)
        {
            if (null == rootKey) rootKey = Registry.LocalMachine;
            try
            {
                var subKey = rootKey.OpenSubKey(subkey, true);
                var subkeyNames = subKey.GetSubKeyNames();
                if (subkeyNames.Any(keyName => keyName == name)) return true;
                subKey.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return false;
        }
    }

}