using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace STDLite.Class
{
    class StandardHelper
    {
        private string GetHtml(string url, string data, Encoding encoding)
        {
            var wc = new WebClient { Encoding = encoding };
            wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");  // 将表单数据转换为键值对
            return wc.UploadString(url, data);
        }


        public List<NationalStandard> GetStandards(string url, string data, Encoding encoding)
        {
            var standards = new List<NationalStandard>();

            var doc = new HtmlDocument();
            doc.LoadHtml(GetHtml(url, data, encoding));
            // 用HtmlAgilityPack解析响应内容
            var nodes = doc.DocumentNode.SelectNodes("//*[contains(@class,'std')] ");
            if (null == nodes)
            {
                standards.Add(new NationalStandard());

                return standards;
            }

            foreach (var node in nodes)
            {
                var standard = new NationalStandard
                {
                    Number = new Regex(@"---(.+)</a></div>").Match(node.InnerHtml).Groups[1].ToString(),
                    Name = new Regex(@">(.+)---").Match(node.InnerHtml).Groups[1].ToString(),
                    // ?指定为懒惰模式
                    State = new Regex(@"标准状态:(.+?)</span>").Match(node.InnerHtml).Groups[1].ToString(),
                    Replacement = new Regex(@":;(.+?)</span>").Match(node.InnerHtml).Groups[1].ToString(),
                    DownloadUrl = "http://10.113.1.69/std/showEBook.aspx?fileid=" +
                    new Regex(@"fileid=(\d{4,})").Match(node.InnerHtml).Groups[1],
                };
                // 正则提取信息
                standards.Add(standard);
            }

            return standards;
        }


        public void DownloadFile(string uri, string filename)
        {
            var wc = new WebClient();
            // 匿名委托使用外部变量，传递文件保存路径
            wc.DownloadFileCompleted += (s, e) => OnDownloadFileCompleted(filename);
            wc.DownloadFileAsync(new Uri(uri), filename);
        }


        private void OnDownloadFileCompleted(string filename)
        {
            var fileLength = (float)new FileInfo(filename).Length / 1024;

            if (fileLength > 10)
            {
                Process.Start(filename);
            }
            else
            {
                File.Delete(filename);
                MessageBox.Show("暂未提供此标准的下载", "下载错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}



