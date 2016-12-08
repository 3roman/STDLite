using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace STDLite.Class
{
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
    /// 继承自IComparer
    /// </summary>
    class ListViewColumnSorter : IComparer
    {
        /// <summary>
        /// 声明CaseInsensitiveComparer类对象
        /// </summary>
        private readonly CaseInsensitiveComparer _objectCompare;

        /// <summary>
        /// 构造函数
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
        /// 获取或设置按照哪一列排序
        /// </summary>
        public int SortColumn { set; get; }

        /// <summary>
        /// 获取或设置排序方式
        /// </summary>
        public SortOrder Order { set; get; }

        /// <summary>
        /// 重写IComparer接口
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
        /// 判断是否为正确的IP地址，IP范围（0.0.0.0～255.255.255）
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
        /// 比较两个数字的大小
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
        /// 比较两个IP地址的大小
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
}
