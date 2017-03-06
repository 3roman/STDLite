using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using STDLite.Class;
using htd = HtmlAgilityPack.HtmlDocument;

namespace STDLite
{
    public partial class FrmMain : Form
    {
        private readonly Func<string, int, int, bool> _mc = Common.CheckPortAlive;
        private readonly StandardHelper _sh = new StandardHelper();

        public FrmMain()
        {
            InitializeComponent();

            Text = string.Format("{0}V{1}   工艺系统室专版",
                Application.ProductName,
                Application.ProductVersion);
            cbxStandard.SelectedIndex = 0;

            lstStandard.ListViewItemSorter = new ListViewColumnSorter();
            lstStandard.ColumnClick += ListViewHelper.ListView_ColumnClick;

            _mc.BeginInvoke("10.113.1.69", 80, 5, AsyncCallbackImpl, null);
        }

        public void AsyncCallbackImpl(IAsyncResult ar)
        {
            var isAlive = _mc.EndInvoke(ar);
            if (!isAlive)
            {
                MessageBox.Show("网站连接不上", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            var keyword = txtKeyword.Text.Replace("/", string.Empty);
            var url = string.Format(0 == cbxStandard.SelectedIndex ? 
                "http://10.113.1.69/std/stdsearch.aspx?key={0}&idx=1" : 
                "http://10.113.1.69/std/stdsearch.aspx?key={0}&idx=0", keyword);
            const string data = "__EVENTTARGET=stdlist%24lbtn_refresh_1&__VIEWSTATE=%2FwEPDwULLTE2NjMyMjA0OTYPZBYCAgEPZBYEZg8PZBYCHgVzdHlsZQUkbWFyZ2luOjBweDtwYWRkaW5nOjBweDtkaXNwbGF5Om5vbmU7ZAIBDw9kFgIfAAUkZGlzcGxheTpub25lO21hcmdpbjowcHg7cGFkZGluZzowcHg7ZBgBBR5fX0NvbnRyb2xzUmVxdWlyZVBvc3RCYWNrS2V5X18WCwUdc3RkbGlzdCRTb3J0RmllbGRfUGVyZm9ybWRhdGUFHXN0ZGxpc3QkU29ydEZpZWxkX1B1Ymxpc2hkYXRlBR1zdGRsaXN0JFNvcnRGaWVsZF9QdWJsaXNoZGF0ZQUdc3RkbGlzdCRTb3J0RmllbGRfRXhwaXJlZGRhdGUFHXN0ZGxpc3QkU29ydEZpZWxkX0V4cGlyZWRkYXRlBRxzdGRsaXN0JFNvcnRGaWVsZF9QZXJtaXRkZXB0BRxzdGRsaXN0JFNvcnRGaWVsZF9QZXJtaXRkZXB0BRlzdGRsaXN0JFNvcnRGaWVsZF9TdGRjb2RlBRlzdGRsaXN0JFNvcnRGaWVsZF9TdGRjb2RlBRtzdGRsaXN0JFNvZnRGaWVsZF9WaWV3Q291bnQFG3N0ZGxpc3QkU29mdEZpZWxkX1ZpZXdDb3VudO8M73tN9IyZEmIzbo3Rt9RG6tux&__EVENTVALIDATION=%2FwEWrwECzvCcwAMCh6Xl4AEC%2B6S9uAQCi7aBzQgCh%2Fe%2B9gYCkffzwAQCwPTZjgcC4KGK2QkCwPTdjgcCvbOBiA4CyPmN0AICl%2FjiqgIC4fuD2g0CnbGyoA4Cov%2BOnQQC88Lr2QYCydjo2QYCisvhqg0C6snGGQL%2B5ZDiAwLqyco5Av7llIIEAurJnvABAv7luLkFAurJopACAv7lvNkFAurJlrABAv7loPkEAurJmtABAv7lpJkFAurJrukCAv7liLIGAurJsokDAv7ljNIGAurJprACAv7lsPkFAurJqtACAv7ltJkGAu%2BCxxkC956R4gMC74LLOQL3npWCBALvgp%2FwAQL3nrm5BQLvgqOQAgL3nr3ZBQLvgpewAQL3nqH5BALvgpvQAQL3nqWZBQLvgq%2FpAgL3nomyBgLvgrOJAwL3no3SBgLvgqewAgL3nrH5BQLvgqvQAgL3nrWZBgLw%2BsYZAviWkeIDAvD6yjkC%2BJaVggQC8Pqe8AEC%2BJa5uQUC8PqikAIC%2BJa92QUC8PqWsAEC%2BJah%2BQQC8Pqa0AEC%2BJalmQUC8Pqu6QIC%2BJaJsgYC8PqyiQMC%2BJaN0gYC8PqmsAIC%2BJax%2BQUC8Pqq0AIC%2BJa1mQYCtfXGGQLBkZHiAwK19co5AsGRlYIEArX1nvABAsGRubkFArX1opACAsGRvdkFArX1lrABAsGRofkEArX1mtABAsGRpZkFArX1rukCAsGRibIGArX1sokDAsGRjdIGArX1prACAsGRsfkFArX1qtACAsGRtZkGArbtxhkCwomR4gMCtu3KOQLCiZWCBAK27Z7wAQLCibm5BQK27aKQAgLCib3ZBQK27ZawAQLCiaH5BAK27ZrQAQLCiaWZBQK27a7pAgLCiYmyBgK27bKJAwLCiY3SBgK27aawAgLCibH5BQK27arQAgLCibWZBgLrpMcZAvvAkeIDAuukyzkC%2B8CVggQC66Sf8AEC%2B8C5uQUC66SjkAIC%2B8C92QUC66SXsAEC%2B8Ch%2BQQC66Sb0AEC%2B8ClmQUC66Sv6QIC%2B8CJsgYC66SziQMC%2B8CN0gYC66SnsAIC%2B8Cx%2BQUC66Sr0AIC%2B8C1mQYC7JzHGQL8uJHiAwLsnMs5Avy4lYIEAuycn%2FABAvy4ubkFAuyco5ACAvy4vdkFAuycl7ABAvy4ofkEAuycm9ABAvy4pZkFAuycr%2BkCAvy4ibIGAuycs4kDAvy4jdIGAuycp7ACAvy4sfkFAuycq9ACAvy4tZkGAvGFxxkChaKR4gMC8YXLOQKFopWCBALxhZ%2FwAQKForm5BQLxhaOQAgKFor3ZBQLxhZewAQKFoqH5BALxhZvQAQKFoqWZBQKY94%2FECgLA9OGOBwL3oYbCBgLA9MWOBwLm2svIAXjLi6m3LznOSnsBwWkNseeuRdVq&stdlist%24txt_page_count_top=10000";
            var standards = _sh.GetStandards(url, data, Encoding.UTF8);
            FillListView(standards);
            
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
                _sh.DownloadFile(uri, fileName);
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

        private void FillListView(IEnumerable<NationalStandard> standards)
        {
            lstStandard.BeginUpdate();
            lstStandard.Items.Clear();

            foreach (var standard in standards)
            {
                var lvi = new ListViewItem(standard.Number);
                lvi.SubItems.Add(standard.Name);
                lvi.SubItems.Add(standard.State);
                lvi.SubItems.Add(standard.Replacement);
                lvi.SubItems.Add(standard.DownloadUrl);
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
                var downloadUrl = lstStandard.FocusedItem.SubItems[4].Text;
                _sh.DownloadFile(downloadUrl, fileName);
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

