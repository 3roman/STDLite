using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using STDLite.Class;
using htd = HtmlAgilityPack.HtmlDocument;

namespace STDLite
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();

            Text = string.Format("{0}V{1}   工艺系统室专版",
                Application.ProductName,
                Application.ProductVersion);

            lstStandard.ListViewItemSorter = new ListViewColumnSorter();
            lstStandard.ColumnClick += ListViewHelper.ListView_ColumnClick;

        }
       
        private void btnSearch_Click(object sender, EventArgs e)
        {
            var keyword = Regex.Replace(txtKeyword.Text.ToUpper(), "/|T", string.Empty);

            Text = "努力检索中......";
            Cursor = Cursors.WaitCursor;
            const string postData = "__EVENTTARGET=stdlist%24lbtn_refresh_2&__EVENTARGUMENT=&__VIEWSTATE=%2FwEPDwULLTE2NjMyMjA0OTYPZBYCAgEPZBYEZg8PZBYCHgVzdHlsZQUkbWFyZ2luOjBweDtwYWRkaW5nOjBweDtkaXNwbGF5Om5vbmU7ZAIBDw9kFgIfAAUkZGlzcGxheTpub25lO21hcmdpbjowcHg7cGFkZGluZzowcHg7ZBgBBR5fX0NvbnRyb2xzUmVxdWlyZVBvc3RCYWNrS2V5X18WCwUdc3RkbGlzdCRTb3J0RmllbGRfUGVyZm9ybWRhdGUFHXN0ZGxpc3QkU29ydEZpZWxkX1B1Ymxpc2hkYXRlBR1zdGRsaXN0JFNvcnRGaWVsZF9QdWJsaXNoZGF0ZQUdc3RkbGlzdCRTb3J0RmllbGRfRXhwaXJlZGRhdGUFHXN0ZGxpc3QkU29ydEZpZWxkX0V4cGlyZWRkYXRlBRxzdGRsaXN0JFNvcnRGaWVsZF9QZXJtaXRkZXB0BRxzdGRsaXN0JFNvcnRGaWVsZF9QZXJtaXRkZXB0BRlzdGRsaXN0JFNvcnRGaWVsZF9TdGRjb2RlBRlzdGRsaXN0JFNvcnRGaWVsZF9TdGRjb2RlBRtzdGRsaXN0JFNvZnRGaWVsZF9WaWV3Q291bnQFG3N0ZGxpc3QkU29mdEZpZWxkX1ZpZXdDb3VudO8M73tN9IyZEmIzbo3Rt9RG6tux&__EVENTVALIDATION=%2FwEWKwLO8JzAAwKHpeXgAQL7pL24BAKLtoHNCAKH9772BgKR9%2FPABALA9NmOBwLgoYrZCQLA9N2OBwK9s4GIDgLI%2BY3QAgKX%2BOKqAgLh%2B4PaDQKdsbKgDgKi%2F46dBALzwuvZBgLJ2OjZBgKKy%2BGqDQLqycYZAv7lkOIDAurJyjkC%2FuWUggQC6sme8AEC%2FuW4uQUC6smikAIC%2FuW82QUC6smWsAEC%2FuWg%2BQQC6sma0AEC%2FuWkmQUC6smu6QIC%2FuWIsgYC6smyiQMC%2FuWM0gYC6smmsAIC%2FuWw%2BQUC6smq0AIC%2FuW0mQYCmPePxAoCwPThjgcC96GGwgYCwPTFjgcC5trLyAEB9COtPt6LDYSCzyGfSC34U4IzoA%3D%3D&stdlist%24hd_dir=&stdlist%24hd_key=635983975871817969&stdlist%24txt_page_count_top=100000&stdlist%24txt_page_index_top=1&stdlist%24SortFieldGrp1=SortField_Performdate&stdlist%24cboSortDirection=%E9%99%8D%E5%BA%8F&stdlist%24txt_page_count_btm=10&stdlist%24txt_page_index_btm=1";
            // 搜索标准号
            var requestURL = string.Format("http://10.113.1.69/std/stdsearch.aspx?key={0}&idx=1&pcnt=10", keyword);
            var standards = SEG.GetStandards(requestURL, postData);
            // 搜索标准名
            requestURL = string.Format("http://10.113.1.69/std/stdsearch.aspx?key={0}&idx=0&pcnt=10", keyword);
            standards.AddRange(SEG.GetStandards(requestURL, postData));
            // 去重
            standards = standards.Distinct().ToList();

            FillListView(standards);

            Text = string.Format("{0}V{1}   工艺系统室专版",
                Application.ProductName,
                Application.ProductVersion);
            Cursor = Cursors.Default;
        }

        private void lstMain_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var fileName = Environment.CurrentDirectory + "\\" +
                       lstStandard.FocusedItem.SubItems[0].Text.Replace("/", string.Empty) + " " +
                       lstStandard.FocusedItem.SubItems[1].Text + ".pdf";
            if (!File.Exists(fileName))
            {
                var uri = lstStandard.FocusedItem.SubItems[4].Text;
                SEG.DownloadFile(uri, fileName);
            }
            else
            { // 文件已下载，直接打开
                Process.Start(fileName);
            }
        }

        private void txtKeyword_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtKeyword.ForeColor != Color.Black)
            {
                txtKeyword.ForeColor = Color.Black;
                txtKeyword.Clear();
            }
            else if (e.KeyData == Keys.Escape)
            {
                txtKeyword.Clear();
            }
        }

        private void FillListView(IEnumerable<Standard> standards)
        {
            lstStandard.BeginUpdate();
            lstStandard.Items.Clear();

            foreach (var standard in standards)
            {
                var lvi = new ListViewItem(standard.Number);
                lvi.SubItems.Add(standard.Name);
                lvi.SubItems.Add(standard.State);
                lvi.SubItems.Add(standard.Replacement);
                lvi.SubItems.Add(standard.DownloadURL);
                lstStandard.Items.Add(lvi);

                switch (standard.State)
                {
                    case "未实施":
                        lvi.ForeColor = Color.Green;
                        break;
                    case "已过期":
                        lvi.ForeColor = Color.Red;
                        break;
                }
            }

            lstStandard.EndUpdate();
        }

        #region 菜单操作
        private void munCopyNumber_Click(object sender, EventArgs e)
        {
            Common.SetClipboard(lstStandard.FocusedItem.SubItems[0].Text);
        }

        private void munCopyName_Click(object sender, EventArgs e)
        {
            Common.SetClipboard(lstStandard.FocusedItem.SubItems[1].Text);
        }

        private void munCopyStandard_Click(object sender, EventArgs e)
        {
            Common.SetClipboard(lstStandard.FocusedItem.SubItems[0].Text + lstStandard.FocusedItem.SubItems[1].Text);
        }

        private void munSaveAs_Click(object sender, EventArgs e)
        {
            // 构造文件名
            var pdfName = lstStandard.FocusedItem.SubItems[0].Text.Replace("/", string.Empty) + " " + lstStandard.FocusedItem.SubItems[1].Text;
            var sfd = new SaveFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), // 设置初始目录
                Filter = @"PDF文件|*.pdf|所有文件|*.*",
                FileName = pdfName
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                var fileName = sfd.FileName;
                var downloadURL = lstStandard.FocusedItem.SubItems[4].Text;
                SEG.DownloadFile(downloadURL, fileName);
            }

        }

        private void mnuAbout_Click(object sender, EventArgs e)
        {
            Process.Start("http://10.151.130.55/forum.php?mod=viewthread&tid=443&page=1");
        }

        private void mnuTopMost_Click(object sender, EventArgs e)
        {
            TopMost = mnuTopMost.Checked;
        }
        #endregion

    }
}

