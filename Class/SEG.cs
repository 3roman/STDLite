using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace STDLite.Class
{
    static class SEG
    {

        /// <summary>
        /// 获取HTML响应长度
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static long GetContentLength(string url)
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

        /// <summary>
        /// 返回标准对象列表
        /// </summary>
        /// <param name="requestURL">请求的URL</param>
        /// <param name="postData">POST的数据</param>
        public static List<Standard> GetStandards(string requestURL, string postData)
        {
            var doc = new HtmlDocument();

            // 返回POST请求的响应内容
            var wc = new WebClient { Encoding = Encoding.UTF8 };
            //下面一句很关键，否则返回数据不全
            wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            var html = wc.UploadData(new Uri(requestURL), "POST", Encoding.UTF8.GetBytes(postData));
            if (html.Length < 10000)
            {
                MessageBox.Show("SEG标准数据库维护中...", "连接错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
            // 加载网页内容
            doc.LoadHtml(Encoding.UTF8.GetString(html));
            // 用HtmlAgilityPack解析响应内容
            var nodes = doc.DocumentNode.SelectNodes("//a[contains(@href,'/std/stdmgr.aspx')]");
            var standards = new List<Standard>();
            if (null != nodes)
            {
                standards.AddRange(from node in nodes
                                   let innerTexts = node.InnerText.Replace("---", "^").Split('^')
                                   select new Standard
                                   {
                                       // 标准号
                                       Number = innerTexts[1].ToUpper(),
                                       // 标准名
                                       Name = innerTexts[0],
                                       // 下载地址(由特定URL与id组成)
                                       DownloadURL = "http://10.113.1.69/std/showEBook.aspx?fileid=" + node.Attributes["href"].Value.Replace("../std/stdmgr.aspx?fileid=", string.Empty),
                                       // 是否过期
                                       State = node.ParentNode.ParentNode.ChildNodes[13].ChildNodes[1].InnerText.Replace("标准状态:", string.Empty),
                                       Replacement = node.ParentNode.ParentNode.ChildNodes[17].ChildNodes[1].InnerText.Replace("替换信息:", string.Empty).Replace(";", string.Empty)
                                   });
            }

            return standards;
        }

        /// <summary>
        /// 异步下载文件
        /// </summary>
        /// <param name="uri">URI</param>
        /// <param name="filename">存放路径</param>
        public static void DownloadFile(string uri, string filename)
        {
            var wc = new WebClient();
            // 匿名委托使用外部变量，传递文件保存路径
            wc.DownloadFileCompleted += (s, e) => OnDownloadFileCompleted(filename);
            wc.DownloadFileAsync(new Uri(uri), filename);
        }

        /// <summary>
        /// 下载完成回调函数
        /// </summary>
        /// <param name="filename"></param>
        private static void OnDownloadFileCompleted(string filename)
        {
            var fi = new FileInfo(filename);
            var fileLength = (float)fi.Length / 1024;

            if (fileLength > 10)
            {
                Process.Start(filename);
            }
            else
            {
                File.Delete(filename);
                MessageBox.Show("SEG标准库暂未提供此标准的下载", "下载错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
    }
}



