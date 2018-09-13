using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Web;
using System.Text;

namespace GraduationProject
{
    //public struct TaobaoModel
    //{
    //    public string TypeName;
    //    public string ProducName;
    //    public string ItemName;
    //    public Guid TBModelID;
    //}

    

    class CrawlerForTaobao
    {
        public List<KeyValuePair<string, string>> ProductAndUrl = new List<KeyValuePair<string, string>>();

        public CrawlerForTaobao()
        {
            ProductAndUrl = ReadProductAndUrl();
        }
  
        /// <summary>
        /// 爬取商品名
        /// </summary>
        public void GetItemName()
        {
            int count = 1;
            string url = string.Empty;
            string pattern = "raw_title\":\"" + @"([\S]+)" + "\"";                  
            try
            {
                for(int k = 0; k < ProductAndUrl.Count; k++)
                {
                    url = ProductAndUrl[k].Value.Contains("http") ? ProductAndUrl[k].Value : "http:" + ProductAndUrl[k].Value;
                    HttpWebRequest rq = (HttpWebRequest)WebRequest.Create(url);
                    rq.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; QQWubi 133; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; CIBA; InfoPath.2)";
                    HttpWebResponse rp = (HttpWebResponse)rq.GetResponse();
                    StreamReader sr = new StreamReader(rp.GetResponseStream(),Encoding.UTF8);
                    string CrawlerResultFilesPath = @"..\..\CrawlerResultFiles\Taobao\"+ ProductAndUrl[k].Key + ".txt";
                    StreamWriter sw = new StreamWriter(CrawlerResultFilesPath, true, Encoding.UTF8);
                    //Important!!!
                    sw.AutoFlush = true;
                    string readLine = sr.ReadLine();        
                    while (!sr.EndOfStream)
                    {
                        if (readLine.Contains("g_page_config"))
                        {
                            readLine = readLine.Replace("pic_url", "*");
                            string[] rawTitleList = readLine.Split('*');
                            for (int i = 0; i < rawTitleList.Length; i++)
                            {
                                Match m = Regex.Match(rawTitleList[i], pattern);
                                if (!string.IsNullOrEmpty(m.Value))
                                {
                                    string temp1 = m.Value.Replace("raw_title\":\"", "");
                                    string name = temp1.Replace("\",\"", "");
                                    sw.WriteLine(count.ToString() + "*" + name + "*" + ProductAndUrl[k].Key);
                                    Console.WriteLine(count.ToString() + ". " + name);
                                    count++;
                                }
                            }
                        }
                        readLine = sr.ReadLine();
                    }
                    sr.Close();
                    sr.Dispose();
                    sw.Close();
                    sw.Dispose();
                    if((k+1) % 200 == 0)
                    {
                        count = 1;
                    }
                }
            }catch(Exception x)
            {
                throw x;
            }
            
        }

        /// <summary>
        /// 读取淘宝商品的url并构造新的包含数目参数的url
        /// </summary>
        /// <returns></returns>
        public List<KeyValuePair<string, string>> ReadProductAndUrl()
        {
            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();
            StreamReader sr = new StreamReader(@"..\..\ProductUrl\Taobao\url.txt",Encoding.Default);
            while (!sr.EndOfStream)
            {
                string[] srSplit = sr.ReadLine().Split('*');
                KeyValuePair<string, string> temp1 = new KeyValuePair<string, string>(srSplit[0], srSplit[1]);
                result.Add(temp1);
                for (int k = 0; k < 100; k++)
                {
                    string moreUrl = srSplit[1] + "&s=" + ((k + 1) * 40).ToString();
                    KeyValuePair<string, string> temp2 = new KeyValuePair<string, string>(srSplit[0], moreUrl);
                    result.Add(temp2);
                }
            }            
            sr.Close();
            sr.Dispose();
            return result;
        }
    }
}
