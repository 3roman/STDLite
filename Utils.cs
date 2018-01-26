using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System;
using System.Net;
using MyExcel = Microsoft.Office.Interop.Excel;
//
namespace STDLite
{
    static class Utils
    {
        public static void CopytToClipboard(string context)
        {
            Clipboard.Clear();
            Clipboard.SetData(DataFormats.Text, context);
        }

        public static void ScanPort(string host, int port)
        {
            try
            {
                new TcpClient().Connect(IPAddress.Parse(host), port); 
            }
            catch
            {
                MessageBox.Show(@"SEG网站故障");
            }
        }

        public static bool FindChineseCharacters(string content)
        {
            return Regex.IsMatch(content, @"[\u4e00-\u9fa5]");
        }

        public static void ListViewToExcel(ListView listView, string fileName)
        {
            var rowCounts = listView.Items.Count;
            if (rowCounts <= 0 || string.IsNullOrEmpty(fileName))
            {
                MessageBox.Show(@"导出失败，缺少要导出的内容!");
                return;
            }
            var columnCounts = listView.Items[0].SubItems.Count;

            var xlApp = new Microsoft.Office.Interop.Excel.Application
            {
                DefaultFilePath = "",
                DisplayAlerts = true,
                SheetsInNewWorkbook = 1
            };
            var xlBook = xlApp.Workbooks.Add(true);

            int rowIndex = 1;
            int columnIndex = 0;
            //将ListView的列名导入Excel第一行
            foreach (ColumnHeader dc in listView.Columns)
            {
                columnIndex++;
                xlApp.Cells[rowIndex, columnIndex] = dc.Text;
            }
            //将ListView中的数据导入Excel中
            for (var i = 0; i < rowCounts; i++)
            {
                rowIndex++;
                columnIndex = 0;
                for (int j = 0; j < columnCounts; j++)
                {
                    columnIndex++;
                    //注意这个在导出的时候加了"\t" 的目的就是避免导出的数据显示为科学计数法。可以放在每行的首尾。
                    xlApp.Cells[rowIndex, columnIndex] = Convert.ToString(listView.Items[i].SubItems[j].Text) + "\t";
                }
            }
            //例外需要说明的是用strFileName,Excel.XlFileFormat.xlExcel9795保存方式时 当你的Excel版本不是95、97 而是2003、2007 时导出的时候会报一个错误：异常来自 HRESULT:0x800A03EC。 解决办法就是换成strFileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal。
            xlBook.SaveAs(fileName,
                Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            MessageBox.Show(@"导出成功!");
        }

    }


}

