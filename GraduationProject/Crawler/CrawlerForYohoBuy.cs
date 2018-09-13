using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Web;

namespace GraduationProject
{
    //public struct ItemInfo
    //{
    //    public string TypeName;
    //    public string ProductName;
    //    public string ItemName;
    //    public string Price;
    //    public string SplitWordResult;
    //    public string OfficialTypeName;
    //}
    public class CrawlerForYohoBuy
    {
        Dictionary<string, string> maleClothingDictionary = new Dictionary<string, string>();
        Dictionary<string, string> femaleClothingDictionary = new Dictionary<string, string>();
        public CrawlerForYohoBuy()
        {
            maleClothingDictionary.Add("棉衣", "男式冬衣");
            maleClothingDictionary.Add("羽绒服", "男式冬衣");
            maleClothingDictionary.Add("大衣", "男式冬衣");
            maleClothingDictionary.Add("夹克", "男式夹克衫");
            maleClothingDictionary.Add("毛衣", "男式毛线衣");
            maleClothingDictionary.Add("针织衫", "男式毛线衣");
            maleClothingDictionary.Add("卫衣", "男式运动装");
            maleClothingDictionary.Add("防风外套", "男式运动装");
            maleClothingDictionary.Add("风衣", "男式运动装");
            maleClothingDictionary.Add("皮衣", "男式运动装");
            maleClothingDictionary.Add("衬衫", "男式衬衫T恤");
            maleClothingDictionary.Add("套装", "男式运动装");
            maleClothingDictionary.Add("西装", "男式西服");
            maleClothingDictionary.Add("马甲", "男式运动装");
            maleClothingDictionary.Add("T恤", "男式衬衫T恤");
            maleClothingDictionary.Add("POLO", "男式运动装");
            maleClothingDictionary.Add("背心", "男式内衣");
            maleClothingDictionary.Add("休闲裤", "男式裤子");
            maleClothingDictionary.Add("牛仔裤", "男式裤子");
            maleClothingDictionary.Add("运动裤", "男式裤子");
            maleClothingDictionary.Add("西裤", "男式裤子");
            maleClothingDictionary.Add("打底&紧身裤", "男式裤子");
            maleClothingDictionary.Add("短裤", "男式裤子");
            maleClothingDictionary.Add("连体裤", "男式裤子");
            maleClothingDictionary.Add("休闲&运动鞋", "男鞋");
            maleClothingDictionary.Add("靴子", "男鞋");
            maleClothingDictionary.Add("时装鞋", "男鞋");
            maleClothingDictionary.Add("凉鞋&凉拖", "男鞋");
            maleClothingDictionary.Add("保养护理", "男鞋");


            femaleClothingDictionary.Add("棉衣", "女式冬衣");
            femaleClothingDictionary.Add("羽绒服", "女式冬衣");
            femaleClothingDictionary.Add("大衣", "女式冬衣");
            femaleClothingDictionary.Add("夹克", "女式外套");
            femaleClothingDictionary.Add("毛衣", "女式毛线衣");
            femaleClothingDictionary.Add("针织衫", "女式毛线衣");
            femaleClothingDictionary.Add("卫衣", "女式运动装");
            femaleClothingDictionary.Add("防风外套", "女式运动装");
            femaleClothingDictionary.Add("POLO", "女式运动装");
            femaleClothingDictionary.Add("风衣", "女式外套");
            femaleClothingDictionary.Add("皮衣", "女式外套");
            femaleClothingDictionary.Add("衬衫", "女式衬衫T恤");
            femaleClothingDictionary.Add("套装", "女式运动装");
            femaleClothingDictionary.Add("西装", "女式外套");
            femaleClothingDictionary.Add("马甲", "女式运动装");
            femaleClothingDictionary.Add("T恤", "女式衬衫T恤");
            femaleClothingDictionary.Add("背心", "女式内衣");
            femaleClothingDictionary.Add("休闲裤", "女式裤子");
            femaleClothingDictionary.Add("牛仔裤", "女式裤子");
            femaleClothingDictionary.Add("运动裤", "女式裤子");
            femaleClothingDictionary.Add("西裤", "女式裤子");
            femaleClothingDictionary.Add("打底&紧身裤", "女式裤子");
            femaleClothingDictionary.Add("短裤", "女式裤子");
            femaleClothingDictionary.Add("连体裤", "女式裤子");
            femaleClothingDictionary.Add("半身裙", "女式裙子");
            femaleClothingDictionary.Add("连衣裙", "女式裙子");
            femaleClothingDictionary.Add("休闲&运动鞋", "女鞋");
            femaleClothingDictionary.Add("靴子", "女鞋");
            femaleClothingDictionary.Add("时装鞋", "女鞋");
            femaleClothingDictionary.Add("凉鞋&凉拖", "女鞋");
            femaleClothingDictionary.Add("保养护理", "女鞋");
        }
        public List<KeyValuePair<string, string>> GetProductUrl(string typeUrl, int TypeID)
        {
            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();
            string readLine = string.Empty;
            string typePattern = string.Empty;
            string urlPattern = "<a href=" + @"([\S]+)" + " title=" + @"([\S]+)>";
            while (TypeID != 1 && TypeID != 2 && TypeID != 3 && TypeID !=4)
            {
                Console.WriteLine("输入的TypeID有误，1为‘全部上衣’，2为‘全部裤装’，3为‘全部鞋靴’，4为‘全部裙装’，请重新输入：");
                TypeID = Convert.ToInt32(Console.ReadLine());
            }
            switch (TypeID)
            {
                case 1:
                    typePattern = "全部上衣";
                    break;
                case 2:
                    typePattern = "全部裤装";
                    break;
                case 3:
                    typePattern = "全部鞋靴";
                    break;
                case 4:
                    typePattern = "全部裙装";
                    break;
                default:                    
                    break;
            }
            try
            {
                Console.WriteLine("正在爬取商品种类的URL......");             
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(typeUrl);
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; QQWubi 133; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; CIBA; InfoPath.2)";
                request.Headers.Add("Accept-Encoding", "gzip, deflate");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();                
                Stream s = new MemoryStream();
                StreamReader sr = new StreamReader(s);
                using (StreamReader streamReader = new StreamReader(new GZipStream(response.GetResponseStream(), CompressionMode.Decompress), Encoding.UTF8))
                {
                    streamReader.BaseStream.CopyTo(s);
                    s.Position = 0;
                    if (!sr.ReadToEnd().Contains("good-detail-text"))
                    {
                        Console.WriteLine("需要手动刷新页面，填写验证码");
                        Console.ReadKey();
                        request = (HttpWebRequest)WebRequest.Create(HttpUtility.HtmlDecode(typeUrl));
                        request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; QQWubi 133; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; CIBA; InfoPath.2)";
                        request.Headers.Add("Accept-Encoding", "gzip, deflate");
                        response = (HttpWebResponse)request.GetResponse();
                        sr = new StreamReader(new GZipStream(response.GetResponseStream(), CompressionMode.Decompress), Encoding.UTF8);
                    }
                }
                s.Position = 0;
                readLine = sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    Match findType = Regex.Match(readLine, typePattern);
                    if (!string.IsNullOrEmpty(findType.Value))
                    {
                        while(!readLine.Contains("</ul>"))
                        {
                            readLine = sr.ReadLine();
                            Match findUrl = Regex.Match(readLine, urlPattern);
                            if (!string.IsNullOrEmpty(findUrl.Value))
                            {
                                string productNameAndUrl = findUrl.Value.Replace("<a href=\"", "").Replace("\" title=\"", "*").Replace("\">", "");
                                for(int i = 1; i < 11; i++)
                                {
                                    KeyValuePair<string, string> temp = new KeyValuePair<string, string>(productNameAndUrl.Split('*')[1], "https://list.yohobuy.com" + productNameAndUrl.Split('*')[0] + "-pg"+i.ToString());
                                    result.Add(temp);
                                    Console.WriteLine("已爬取到 {0} 的URL", temp.Key);
                                }
                            }
                        }
                        break;
                    }
                    readLine = sr.ReadLine();
                }
                s.Close();
                s.Dispose();
                sr.Close();
                sr.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine( e.StackTrace+"\n"+e.Message);
            }           
            return result;
        }

        //public List<ItemInfoModel> GetItemInfo(List<KeyValuePair<string, string>> productNameAndProductUrl, string gender)
        //{
        //    string itemNamePattern = "<a href=\"//www.yohobuy.com/product" + @"([\S]+)[\s]target=";
        //    //string itemPricePattern = "<span class=\"sale-price prime-cost\">";
        //    string readLine = string.Empty;
        //    int o = 1;
        //    List<ItemInfoModel> result = new List<ItemInfoModel>();
        //    try
        //    {

        //        for (int i = 0; i < productNameAndProductUrl.Count; i++)
        //        {
        //            string officialProductName = MatchOfficialProductName(gender, productNameAndProductUrl[i].Key);
        //            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(HttpUtility.HtmlDecode(productNameAndProductUrl[i].Value));
        //            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; QQWubi 133; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; CIBA; InfoPath.2)";
        //            request.Headers.Add("Accept-Encoding", "gzip, deflate");
        //            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //            Stream s = new MemoryStream();
        //            StreamReader sr = new StreamReader(s);
        //            using (StreamReader streamReader = new StreamReader(new GZipStream(response.GetResponseStream(), CompressionMode.Decompress), Encoding.UTF8))
        //            {
        //                streamReader.BaseStream.CopyTo(s);
        //                s.Position = 0;                        
        //                if (!sr.ReadToEnd().Contains("good-detail-text"))
        //                {
        //                    Console.WriteLine("需要手动刷新页面，填写验证码");
        //                    Console.ReadKey();
        //                    request = (HttpWebRequest)WebRequest.Create(HttpUtility.HtmlDecode(productNameAndProductUrl[i].Value));
        //                    request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; QQWubi 133; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; CIBA; InfoPath.2)";
        //                    request.Headers.Add("Accept-Encoding", "gzip, deflate");
        //                    response = (HttpWebResponse)request.GetResponse();
        //                    sr = new StreamReader(new GZipStream(response.GetResponseStream(), CompressionMode.Decompress), Encoding.UTF8);
        //                }                        
        //            }
        //            s.Position = 0;                                     
        //            readLine = sr.ReadLine();
        //            while (!sr.EndOfStream)
        //            {
        //                Match itemName = Regex.Match(readLine, itemNamePattern);
        //                if (!string.IsNullOrEmpty(itemName.Value))
        //                {
        //                    string name = readLine.Replace(@"blank" + "\">", "*").Split('*')[1].Replace("</a>","");
        //                    ItemInfoModel itemInfo = new ItemInfoModel();
        //                    itemInfo.ItemName = name;
        //                    itemInfo.ProductName = productNameAndProductUrl[i].Key;
        //                    itemInfo.OfficialProductName = officialProductName;
        //                    result.Add(itemInfo);
        //                    Console.WriteLine(o.ToString() + ". " + itemInfo.ItemName + "  " + itemInfo.ProductName + "  " +itemInfo.OfficialProductName);
        //                    o++;
        //                }
        //                //Match itemPrice = Regex.Match(readLine, itemPricePattern);
        //                //if (!string.IsNullOrEmpty(itemPrice.Value))
        //                //{
        //                //    readLine = sr.ReadLine();
        //                //    Console.WriteLine(readLine);
        //                //}
        //                readLine = sr.ReadLine();
        //            }
        //            s.Close();
        //            s.Dispose();
        //            sr.Close();
        //            sr.Dispose();
        //        }

        //    }
        //    catch(Exception e)
        //    {
        //        Console.WriteLine(e.StackTrace + "\n" + e.Message);
        //    }

        //    return result;
        //}

        public void GetItemInfo(List<KeyValuePair<string, string>> productNameAndProductUrl, string gender)
        {
            string itemNamePattern = "<a href=\"//www.yohobuy.com/product" + @"([\S]+)[\s]target=";
            //string itemPricePattern = "<span class=\"sale-price prime-cost\">";
            string readLine = string.Empty;
            string productName = string.Empty;
            int o = 1;
            try
            {                
                for (int i = 0; i < productNameAndProductUrl.Count; i++)
                {
                    productName = productNameAndProductUrl[i].Key.Contains(@"/") ? productNameAndProductUrl[i].Key.Replace(@"/", "&") : productNameAndProductUrl[i].Key;                    
                    string CrawlerResultFilesPath = @"..\..\CrawlerResultFiles\Yoho\" + gender + productName + ".txt";
                    StreamWriter sw = new StreamWriter(CrawlerResultFilesPath, true, Encoding.UTF8);
                    //important!!!
                    sw.AutoFlush = true;
                    string officialProductName = MatchOfficialProductName(gender, productName);
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(HttpUtility.HtmlDecode(productNameAndProductUrl[i].Value));
                    request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; QQWubi 133; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; CIBA; InfoPath.2)";
                    request.Headers.Add("Accept-Encoding", "gzip, deflate");
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream s = new MemoryStream();
                    StreamReader sr = new StreamReader(s);
                    using (StreamReader streamReader = new StreamReader(new GZipStream(response.GetResponseStream(), CompressionMode.Decompress), Encoding.UTF8))
                    {
                        streamReader.BaseStream.CopyTo(s);
                        s.Position = 0;
                        string readToEnd = sr.ReadToEnd();
                        if (!readToEnd.Contains("good-detail-text"))
                        {
                            if (readToEnd.Contains("抱歉！未找到相关商品"))
                            {
                                Console.WriteLine("当前类型已经爬取完毕");
                                s.Close();
                                s.Dispose();
                                sr.Close();
                                sr.Dispose();
                                sw.Close();
                                sw.Dispose();
                                if ((i + 1) % 10 == 0)
                                {
                                    o = 1;
                                }
                                continue;
                            }
                            Console.WriteLine("需要手动刷新页面，填写验证码");
                            Console.ReadKey();
                            request = (HttpWebRequest)WebRequest.Create(HttpUtility.HtmlDecode(productNameAndProductUrl[i].Value));
                            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; QQWubi 133; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; CIBA; InfoPath.2)";
                            request.Headers.Add("Accept-Encoding", "gzip, deflate");
                            response = (HttpWebResponse)request.GetResponse();
                            sr = new StreamReader(new GZipStream(response.GetResponseStream(), CompressionMode.Decompress), Encoding.UTF8);
                        }
                    }
                    s.Position = 0;
                    readLine = sr.ReadLine();
                    while (!sr.EndOfStream)
                    {
                        Match itemName = Regex.Match(readLine, itemNamePattern);
                        if (!string.IsNullOrEmpty(itemName.Value))
                        {
                            string name = readLine.Replace(@"blank" + "\">", "*").Split('*')[1].Replace("</a>", "");
                            //ItemInfoModel itemInfo = new ItemInfoModel();
                            //itemInfo.ItemName = name;
                            //itemInfo.ProductName = productNameAndProductUrl[i].Key;
                            //itemInfo.OfficialProductName = officialProductName;
                            //result.Add(itemInfo);
                            //Console.WriteLine(o.ToString() + ". " + itemInfo.ItemName + "  " + itemInfo.ProductName + "  " + itemInfo.OfficialProductName);
                            string forswToWrite = o.ToString() + "*" + name + "*" + productNameAndProductUrl[i].Key + "*" + officialProductName;
                            //foreach (char c in itemchar)
                            //{
                            //    sw.Write(c);
                            //}
                            sw.WriteLine(forswToWrite);
                            o++;
                        }
                        //商品价格
                        //Match itemPrice = Regex.Match(readLine, itemPricePattern);
                        //if (!string.IsNullOrEmpty(itemPrice.Value))
                        //{
                        //    readLine = sr.ReadLine();
                        //    Console.WriteLine(readLine);
                        //}
                        readLine = sr.ReadLine();
                    }
                    s.Close();
                    s.Dispose();
                    sr.Close();
                    sr.Dispose();
                    sw.Close();
                    sw.Dispose();
                    if ((i+1)%10 == 0)
                    {
                        o = 1;
                    }                    
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace + "\n" + e.Message);
            }
        }

        //查询字典，确定官方分类
        public string MatchOfficialProductName(string gender, string productName)
        {
            string officialProductName = string.Empty;
            if (gender == "male")
            {
                officialProductName = maleClothingDictionary[productName];
            }else if(gender == "female")
            {
                officialProductName = femaleClothingDictionary[productName];
            }            
            return officialProductName;
        }


        //爬取商品URL并保存到本地
        public void WriteProductAndUrl(List<KeyValuePair<string, string>> productNameAndProductUrl,string typeName)
        {
            StreamWriter sw = new StreamWriter(@"..\..\ProductUrl\Yoho\"+typeName+"url.txt",true,Encoding.UTF8);
            for(int i = 0; i < productNameAndProductUrl.Count; i++)
            {
                sw.WriteLine(productNameAndProductUrl[i].Key + "*" + productNameAndProductUrl[i].Value);
            }
            sw.Close();
            sw.Dispose();
        }


        //读取本地文件中爬取到的商品URL并返回
        public List<KeyValuePair<string, string>> ReadProductAndUrl(string typeName)
        {
            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();
            StreamReader sr = new StreamReader(@"..\..\ProductUrl\Yoho\" + typeName + "url.txt");
            for (int i = 0; i < 10; i++)
            {
                string[] srSplit = sr.ReadLine().Split('*');
                KeyValuePair<string, string> temp = new KeyValuePair<string, string>(srSplit[0], srSplit[1]);
                result.Add(temp);
            }
            sr.Close();
            sr.Dispose();
            return result;
        }
        
    }
        
}
