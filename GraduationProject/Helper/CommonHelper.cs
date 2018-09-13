using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Collections;

namespace GraduationProject
{
    static class CommonHelper
    {
        static Dictionary<string, string> EnToCn = new Dictionary<string, string>();
        static CommonHelper()
        {            
            EnToCn.Add("femaleCoat", "女式外套");
            EnToCn.Add("femaleDress", "女式裙子");
            EnToCn.Add("femalePants", "女式裤子");
            EnToCn.Add("femaleShirt", "女式衬衫T恤");
            EnToCn.Add("femaleSportswear", "女式运动装");
            EnToCn.Add("femaleSweater", "女式毛线衣");
            EnToCn.Add("femaleUnderwear", "女式内衣");
            EnToCn.Add("femaleWinterClothes", "女式冬衣");
            EnToCn.Add("maleJacket", "男式夹克衫");
            EnToCn.Add("malePants", "男式裤子");
            EnToCn.Add("maleShirt", "男式衬衫T恤");
            EnToCn.Add("maleSportswear", "男式运动装");
            EnToCn.Add("maleSuit", "男式西服");
            EnToCn.Add("maleSweater", "男式毛线衣");
            EnToCn.Add("maleUnderwear", "男式内衣");
            EnToCn.Add("maleWinterClothes", "男式冬衣");
        }

        /// <summary>
        /// 将淘宝和有货爬下来的数据存在一起
        /// </summary>
        static public void GetTotalCrawlerResultFiles()
        {
            string taobaoPath = @"..\..\CrawlerResultFiles\Taobao";
            string yohoPath = @"..\..\CrawlerResultFiles\Yoho";
            string totalPath = @"..\..\CrawlerResultFiles\Total\";
            string[] taobaoFiles = Directory.GetFiles(taobaoPath);
            string[] yohoFiles = Directory.GetFiles(yohoPath);
            //string maleSuit, maleWinterClothes, maleSweater, maleJacket, maleSportswear, maleShirt, malePants, maleUnderwear, femaleCoat, femaleWinterClothes, femaleSweater, femaleSportswear, femaleShirt, femaleDress, femalePants, femaleUnderwear;
            TypeModel type = new TypeModel();
            try
            {
                foreach(string fileName in taobaoFiles)
                {
                    StreamReader taobaoSR = new StreamReader(fileName);
                    string readToEnd = taobaoSR.ReadToEnd();
                    if (readToEnd.Contains("男式衬衫T恤"))
                    {
                        type.maleShirt = readToEnd+"\r\n";
                    }else if (readToEnd.Contains("男式冬衣"))
                    {
                        type.maleWinterClothes = readToEnd + "\r\n";
                    }else if (readToEnd.Contains("男式夹克衫"))
                    {
                        type.maleJacket = readToEnd + "\r\n";
                    }else if (readToEnd.Contains("男式裤子"))
                    {
                        type.malePants = readToEnd + "\r\n";
                    }else if (readToEnd.Contains("男式毛线衣"))
                    {
                        type.maleSweater = readToEnd + "\r\n";
                    }else if (readToEnd.Contains("男式内衣"))
                    {
                        type.maleUnderwear = readToEnd + "\r\n";
                    }else if (readToEnd.Contains("男式西服"))
                    {
                        type.maleSuit = readToEnd + "\r\n";
                    }else if (readToEnd.Contains("男式运动装"))
                    {
                        type.maleSportswear = readToEnd + "\r\n";
                    }else if (readToEnd.Contains("女式衬衫T恤"))
                    {
                        type.femaleShirt = readToEnd + "\r\n";
                    }else if (readToEnd.Contains("女式冬衣"))
                    {
                        type.femaleWinterClothes = readToEnd + "\r\n";
                    }else if (readToEnd.Contains("女式裤子"))
                    {
                        type.femalePants = readToEnd + "\r\n";
                    }else if (readToEnd.Contains("女式毛线衣"))
                    {
                        type.femaleSweater = readToEnd + "\r\n";
                    }else if (readToEnd.Contains("女式内衣"))
                    {
                        type.femaleUnderwear = readToEnd + "\r\n";
                    }else if (readToEnd.Contains("女式裙子"))
                    {
                        type.femaleDress = readToEnd + "\r\n";
                    }else if (readToEnd.Contains("女式运动装"))
                    {
                        type.femaleCoat = readToEnd + "\r\n";
                    }else if (readToEnd.Contains("女式外套"))
                    {
                        type.femaleSportswear = readToEnd + "\r\n";
                    }
                    taobaoSR.Close();
                    taobaoSR.Dispose();
                }

                Console.WriteLine("淘宝读取完成");

                foreach (string fileName in yohoFiles)
                {
                    StreamReader yohoSR = new StreamReader(fileName);
                    string readLine = string.Empty;
                    string readToEnd = string.Empty;
                    while (!yohoSR.EndOfStream)
                    {
                        readLine = yohoSR.ReadLine();
                        string[] temp = readLine.Split('*');
                        readToEnd += temp[0] + "*" + temp[1] + "*" + temp[3] + "\r\n";
                    }                    
                    if (readToEnd.Contains("男式衬衫T恤"))
                    {
                        type.maleShirt += readToEnd;
                    }
                    else if (readToEnd.Contains("男式冬衣"))
                    {
                        type.maleWinterClothes += readToEnd;
                    }
                    else if (readToEnd.Contains("男式夹克衫"))
                    {
                        type.maleJacket += readToEnd;
                    }
                    else if (readToEnd.Contains("男式裤子"))
                    {
                        type.malePants += readToEnd;
                    }
                    else if (readToEnd.Contains("男式毛线衣"))
                    {
                        type.maleSweater += readToEnd;
                    }
                    else if (readToEnd.Contains("男式内衣"))
                    {
                        type.maleUnderwear += readToEnd;
                    }
                    else if (readToEnd.Contains("男式西服"))
                    {
                        type.maleSuit += readToEnd;
                    }
                    else if (readToEnd.Contains("男式运动装"))
                    {
                        type.maleSportswear += readToEnd;
                    }
                    else if (readToEnd.Contains("女式衬衫T恤"))
                    {
                        type.femaleShirt += readToEnd;
                    }
                    else if (readToEnd.Contains("女式冬衣"))
                    {
                        type.femaleWinterClothes += readToEnd;
                    }
                    else if (readToEnd.Contains("女式裤子"))
                    {
                        type.femalePants += readToEnd;
                    }
                    else if (readToEnd.Contains("女式毛线衣"))
                    {
                        type.femaleSweater += readToEnd;
                    }
                    else if (readToEnd.Contains("女式内衣"))
                    {
                        type.femaleUnderwear += readToEnd;
                    }
                    else if (readToEnd.Contains("女式裙子"))
                    {
                        type.femaleDress += readToEnd;
                    }
                    else if (readToEnd.Contains("女式运动装"))
                    {
                        type.femaleCoat += readToEnd;
                    }
                    else if (readToEnd.Contains("女式外套"))
                    {
                        type.femaleSportswear += readToEnd;
                    }
                    yohoSR.Close();
                    yohoSR.Dispose();
                }

                Console.WriteLine("有货读取完成");

                string[] result = TypeModel.GetTypeNameStringArray();
                foreach(string item in result)
                {
                    string path = totalPath + item + ".txt";
                    StreamWriter sw = new StreamWriter(path, true, Encoding.Default);
                    sw.AutoFlush = true;
                    if (item == "maleShirt")
                    {
                        sw.WriteLine(type.maleShirt);
                    }
                    else if (item == "maleWinterClothes")
                    {
                        sw.WriteLine(type.maleWinterClothes);
                    }
                    else if (item == "maleJacket")
                    {
                        sw.WriteLine(type.maleJacket);
                    }
                    else if (item == "malePants")
                    {
                        sw.WriteLine(type.malePants);
                    }
                    else if (item == "maleSweater")
                    {
                        sw.WriteLine(type.maleSweater);
                    }
                    else if (item == "maleUnderwear")
                    {
                        sw.WriteLine(type.maleUnderwear);
                    }
                    else if (item == "maleSuit")
                    {
                        sw.WriteLine(type.maleSuit);
                    }
                    else if (item == "maleSportswear")
                    {
                        sw.WriteLine(type.maleSportswear);
                    }
                    else if (item == "femaleShirt")
                    {
                        sw.WriteLine(type.femaleShirt);
                    }
                    else if (item == "femaleWinterClothes")
                    {
                        sw.WriteLine(type.femaleWinterClothes);
                    }
                    else if (item == "femalePants")
                    {
                        sw.WriteLine(type.femalePants);
                    }
                    else if (item == "femaleSweater")
                    {
                        sw.WriteLine(type.femaleSweater);
                    }
                    else if (item == "femaleUnderwear")
                    {
                        sw.WriteLine(type.femaleUnderwear);
                    }
                    else if (item == "femaleDress")
                    {
                        sw.WriteLine(type.femaleDress);
                    }
                    else if (item == "femaleCoat")
                    {
                        sw.WriteLine(type.femaleCoat);
                    }
                    else if (item == "femaleSportswear")
                    {
                        sw.WriteLine(type.femaleSportswear);
                    }
                    sw.Close();
                    sw.Dispose();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.ReadKey();
        }

        /// <summary>
        /// 爬虫
        /// </summary>
        static public void Crawler()
        {
            //string url = "https://www.taobao.com/markets/nanzhuang/2017new?spm=a21bo.2017.201867-main.2.30d15053ClMljq";
            CrawlerForTaobao taobaoCrawler = new CrawlerForTaobao();
            taobaoCrawler.GetItemName();
            Console.WriteLine("爬取完成");
            Console.ReadKey();



            ///Summary
            ///Yoho
            //string maleClothingUrl = ConfigurationManager.AppSettings["MaleClothingUrl"].ToString();
            //string femaleClothingUrl = ConfigurationManager.AppSettings["femaleClothingUrl"].ToString();
            //CrawlerForYohoBuy crawler = new CrawlerForYohoBuy();

            //1为‘全部上衣’，2为‘全部裤装’，3为‘全部鞋靴’，4为‘全部裙装’

            //crawler.WriteProductAndUrl(crawler.GetProductUrl(femaleClothingUrl, 1),"女士上衣");
            //Console.WriteLine(crawler.GetItemInfo(crawler.ReadProductAndUrl("女士上衣"), "female"));
            //crawler.WriteProductAndUrl(crawler.GetProductUrl(maleClothingUrl,3), "男士鞋靴");
            //crawler.GetItemInfo(crawler.GetProductUrl(maleClothingUrl,3), "male");
            //Console.WriteLine("OK");
            //Console.ReadKey();
        }

        /// <summary>
        /// 将所有数据重新计数
        /// </summary>
        static public void ReCount()
        {
            
            string swpath = @"..\..\CrawlerResultFiles\TestData\";
            //string srpath1 = @"..\..\CrawlerResultFiles\TestData\bk\";
            //string srpath2 = @"..\..\CrawlerResultFiles\TestData\bk2\";            
            string srpath = @"..\..\CrawlerResultFiles\TestData\total\";

            int total = 0;
            foreach(string file in TypeModel.GetTypeNameStringArray())
            {
                int i = 1;
                Console.WriteLine(file + "正在去重并计数");
                string readLine = string.Empty;
                string toWrite = string.Empty;
                string readToEnd = string.Empty;
                //StreamReader sr1 = new StreamReader(srpath1 + file + ".txt", Encoding.Default);
                //StreamReader sr2 = new StreamReader(srpath2 + file + ".txt", Encoding.Default);
                //readToEnd += sr1.ReadToEnd();
                //readToEnd += sr2.ReadToEnd();
                //sr1.Close();
                //sr1.Dispose();
                //sr2.Close();
                //sr2.Dispose();
                StreamReader sr = new StreamReader(srpath + file + ".txt", Encoding.Default);
                readToEnd = sr.ReadToEnd();
                string[] lines = readToEnd.Split('\r').Where(s => !string.IsNullOrEmpty(s)).ToArray();
                StreamWriter sw = new StreamWriter(swpath + file + ".txt", true, Encoding.Default);
                for(int k = 0;k<lines.Length;k++)
                {
                    if (lines[k] == "\n") continue;
                    string[] temp = lines[k].Split('*');
                    if (toWrite.Contains(temp[1]))
                    {
                        continue;
                    }
                    else
                    {
                        toWrite += i.ToString() + "*" + temp[1] + "*" + temp[2] + "\r\n";
                    }
                    i++;
                }                                      
                
                sw.WriteLine(toWrite);
                sr.Close();
                sr.Dispose();
                sw.Close();
                sw.Dispose();
                total += i;
                Console.WriteLine(file + "  共爬到了  " + i.ToString() + "\r\n");
            }
            Console.WriteLine("所有数据共有" + total.ToString());
        }


        /// <summary>
        /// 传统TF-IDF算法
        /// </summary>
        //static public void TFIDF()
        //{
        //    List<string> allFilesDimensionSum = new List<string>();
        //    string srpath = @"..\..\SplitWordResultFiles\";
        //    string swpath = @"..\..\TF-IDF\";
        //    int docuNum = TypeModel.GetTypeNameStringArray().Length+1;
        //    try
        //    {
        //        //读取所有文档中非重复的分词结果，拼成字符串,用于计算idf
        //        foreach (string fileName in TypeModel.GetTypeNameStringArray())
        //        {
        //            List<string> dimension = new List<string>();
        //            StreamReader sr = new StreamReader(srpath + fileName + ".txt", Encoding.Default);
        //            string readLine = sr.ReadLine();
        //            while (!sr.EndOfStream)
        //            {
        //                foreach (string word in readLine.Split(' ').Where(s => !string.IsNullOrEmpty(s)).ToArray())
        //                {
        //                    if (!dimension.Contains(word))
        //                    {
        //                        dimension.Add(word);
        //                        allFilesDimensionSum.Add(word);
        //                    }
        //                }
        //                readLine = sr.ReadLine();
        //            }
        //            sr.Close();
        //            sr.Dispose();
        //        }
        //        Console.WriteLine("读取所有文档中非重复的分词结果完成！");

        //        //计算TF-IDF值
        //        foreach (string fileName in TypeModel.GetTypeNameStringArray())
        //        {
        //            Console.WriteLine("正在计算" + fileName + "的tf-idf值：");
        //            List<string> dimension = new List<string>();
        //            List<string> oneFileWords = new List<string>();
        //            Dictionary<string, double> noLimitDimension = new Dictionary<string, double>();

        //            List<KeyValuePair<string,double>> TF_IDF = new List<KeyValuePair<string,double>>();
        //            int oneFileWordsNum = 0;                    
        //            StreamReader sr = new StreamReader(srpath + fileName + ".txt", Encoding.Default);
        //            string readLine = sr.ReadLine();
        //            while (!sr.EndOfStream)
        //            {
        //                oneFileWords.AddRange(readLine.Split(' ').Where(s => !string.IsNullOrEmpty(s)).ToList());
        //                foreach (string word in readLine.Split(' ').Where(s => !string.IsNullOrEmpty(s)).ToArray())
        //                {
        //                    if (!dimension.Contains(word))
        //                    {
        //                        dimension.Add(word);
        //                    }
        //                }
        //                readLine = sr.ReadLine();
        //            }
        //            oneFileWordsNum = oneFileWords.Count;
        //            //计算每个词的TF-IDF
        //            for(int i = 0; i < dimension.Count; i++)
        //            {
        //                if (string.IsNullOrEmpty(dimension[i])) continue;
        //                string pattern = dimension[i];
        //                //MatchCollection m1 = Regex.Matches(readToEnd, pattern);
        //                //MatchCollection m2 = Regex.Matches(allFilesDimensionSum, pattern);
        //                int match1 = 0, match2 = 0;
        //                foreach (string m in oneFileWords)
        //                {
        //                    if (m == pattern) match1++; 
        //                }
        //                foreach(string m in allFilesDimensionSum)
        //                {
        //                    if (m == pattern) match2++;
        //                }
        //                if (dimension[i] == "男/b"|| dimension[i] == "女/b")
        //                {
        //                    match2--;
        //                }
        //                double tf = (double)match1 / oneFileWordsNum;
        //                double idf = Math.Log((double)docuNum / (match2));
        //                double tfidf = tf * idf;

        //                #region 增加权重部分
        //                //string[] zhname = EnToCn[fileName].Split('式');      //增加性别权重        
        //                //if (zhname[1].Contains("运动")) zhname[1] = zhname[1].Replace("装", "");
        //                //if (zhname[1].Contains("裤")) zhname[1] = zhname[1].Replace("子", "");
        //                //if (zhname[1].Contains("裙")) zhname[1] = zhname[1].Replace("子", "");
        //                //if (zhname[1].Contains("毛线衣")) zhname[1] = zhname[1].Replace("线", "");
        //                //for (int j = 0; j < zhname.Length-1; j++)
        //                //{
        //                //    if (dimension[i].Contains(zhname[j]))
        //                //    {
        //                //        tfidf = tfidf * 2;
        //                //    }
        //                //}
        //                #endregion

        //                if (tfidf > 0 && !dimension[i].Contains('@'))
        //                {
        //                    noLimitDimension.Add(dimension[i], tfidf);
        //                    //TF_IDF.Add(new KeyValuePair<string, double>(dimension[i], tfidf));
        //                }                                              
        //            }


        //            TF_IDF = SortedByValue(noLimitDimension);

        //            Console.WriteLine("正在写入" + fileName + "的TF-IDF值……");
        //            TFIDFJson result = new TFIDFJson();
        //            result.Type = fileName;
        //            result.Data = TF_IDF;
        //            string toWrite = JsonConvert.SerializeObject(result);
        //            StreamWriter sw = new StreamWriter(swpath + fileName + ".txt", true, Encoding.Default);
        //            sw.Write(toWrite);
        //            sw.Close();
        //            sw.Dispose();
        //            Console.WriteLine("写入" + fileName + "成功! \r\n");                    
        //        }
        //        Console.WriteLine("完成所有文本的TF-IDF值计算！");
        //    }
        //    catch(Exception ex)
        //    {
        //        //Console.WriteLine("出现异常：" +ex.Message);
        //        throw ex;
        //    }

        //}


        /// <summary>
        /// 改进TF-IDF算法
        /// </summary>
        static public void TFIDF()
        {
            List<List<string>> allFilesWords = new List<List<string>>();
            int allFilesTrainDataNum = 0;
            string srpath = @"..\..\SplitWordResultFiles\";
            string swpath = @"..\..\TF-IDF\";
            int docuNum = TypeModel.GetTypeNameStringArray().Length + 1;
            try
            {
                //读取所有文档中非重复的分词结果，拼成字符串,用于计算idf
                foreach (string fileName in TypeModel.GetTypeNameStringArray())
                {
                    List<string> dimension = new List<string>();
                    StreamReader sr = new StreamReader(srpath + fileName + ".txt", Encoding.Default);
                    string readLine = sr.ReadLine();
                    while (!sr.EndOfStream)
                    {
                        allFilesTrainDataNum++;
                        dimension.AddRange(readLine.Split(' ').Where(s => !string.IsNullOrEmpty(s)).ToList());                                
                        readLine = sr.ReadLine();
                    }
                    allFilesWords.Add(dimension);
                    sr.Close();
                    sr.Dispose();
                }
                Console.WriteLine("读取所有文档中非重复的分词结果完成！");

                //计算TF-IDF值
                foreach (string fileName in TypeModel.GetTypeNameStringArray())
                {
                    Console.WriteLine("正在计算" + fileName + "的tf-idf值：");
                    List<string> dimension = new List<string>();
                    List<string> oneFileWords = new List<string>();
                    Dictionary<string, double> noLimitDimension = new Dictionary<string, double>();

                    List<KeyValuePair<string, double>> TF_IDF = new List<KeyValuePair<string, double>>();
                    int oneFileWordsNum = 0;
                    int oneFileTrainDataNum = 0;
                    StreamReader sr = new StreamReader(srpath + fileName + ".txt", Encoding.Default);
                    string readLine = sr.ReadLine();
                    while (!sr.EndOfStream)
                    {
                        oneFileTrainDataNum++;
                        oneFileWords.AddRange(readLine.Split(' ').Where(s => !string.IsNullOrEmpty(s)).ToList());
                        foreach (string word in readLine.Split(' ').Where(s => !string.IsNullOrEmpty(s)).ToArray())
                        {
                            if (!dimension.Contains(word))
                            {
                                dimension.Add(word);
                            }
                        }
                        readLine = sr.ReadLine();
                    }
                    oneFileWordsNum = oneFileWords.Count;
                    //计算每个词的TF-IDF
                    for (int i = 0; i < dimension.Count; i++)
                    {
                        if (string.IsNullOrEmpty(dimension[i])) continue;
                        string pattern = dimension[i];
                        //MatchCollection m1 = Regex.Matches(readToEnd, pattern);
                        //MatchCollection m2 = Regex.Matches(allFilesDimensionSum, pattern);
                        int match1 = 0, match2 = 0;
                        foreach (string m in oneFileWords)
                        {
                            if (m == pattern) match1++;
                        }
                        foreach (List<string> m in allFilesWords)
                        {
                            foreach (string n in m)
                            {
                                if (n == pattern) match2++;
                            }                             
                        }
                        int antiThisClassNum = allFilesTrainDataNum - oneFileTrainDataNum;
                        int antiThisClassMatchNum = match2 - match1;                        
                        double antiThisClassMatchFactor = (double)antiThisClassMatchNum / antiThisClassNum;
                        double thisClassMatchFactor = (double)match1 / oneFileTrainDataNum;
                        double tf = (double)match1 / oneFileWordsNum;
                        double idfFactor = (double)match1 / match2 * allFilesTrainDataNum;
                        double idf = Math.Log(idfFactor);
                        double tfidf = tf * idf;

                        if (tfidf > 0 && !dimension[i].Contains('@'))
                        {
                            noLimitDimension.Add(dimension[i], tfidf);
                        }
                    }


                    TF_IDF = SortedByValue(noLimitDimension);

                    Console.WriteLine("正在写入" + fileName + "的TF-IDF值……");
                    TFIDFJson result = new TFIDFJson();
                    result.Type = fileName;
                    result.Data = TF_IDF;
                    string toWrite = JsonConvert.SerializeObject(result);
                    StreamWriter sw = new StreamWriter(swpath + fileName + ".txt", true, Encoding.Default);
                    sw.Write(toWrite);
                    sw.Close();
                    sw.Dispose();
                    Console.WriteLine("写入" + fileName + "成功! \r\n");
                }
                Console.WriteLine("完成所有文本的TF-IDF值计算！");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        static double InformationEntropy (double[] frequency)
        {
            return 0;
        }


        static List<KeyValuePair<string,double>> SortedByValue(Dictionary<string,double> di)
        {
            try
            {
                List<KeyValuePair<string, double>> result = new List<KeyValuePair<string, double>>();
                string[] keys = new string[di.Count];
                double[] values = new double[di.Count];
                double[] newValues = new double[di.Count];
                double count = 0;
                di.Keys.CopyTo(keys, 0);
                di.Values.CopyTo(values, 0);
                Array.Sort(values, keys);
                for (int i = 1; i < 601; i++)
                {
                    count += Math.Pow(values[values.Length - i],2);
                }
                for (int i = 1; i < 601; i++)
                {
                    newValues[newValues.Length - i] = values[values.Length - i] / Math.Sqrt(count);
                    result.Add(new KeyValuePair<string, double>(keys[keys.Length - i], newValues[keys.Length - i]));
                }
                return result;
            }catch(Exception ex)
            {
                throw ex;
            }            
        }

        /// <summary>
            /// 余弦相似度
            /// </summary>
            /// <param name="itemName">商品名</param>
            /// <param name="Corpus">语料</param>
            /// <returns></returns>
        public static double CosSimilarity(string itemName,string Corpus)
        {
            double Ew2 = 0, Eb2 = 0, Ewb = 0;
            double cosSimilarity = 0;
            string splitword = SplitWordHelper.RemoveSplitWord(SplitWordHelper.SplitWords(itemName));
            string[] swarray = splitword.Split(' ').Where(s => !string.IsNullOrEmpty(s)).ToArray();
            Dictionary<string,double> item = new Dictionary<string,double>();
            TFIDFJson tfidf = JsonConvert.DeserializeObject<TFIDFJson>(Corpus);
            List<KeyValuePair<string,double>> corpus = tfidf.Data;

            foreach (string sw in swarray)
            {
                if (!string.IsNullOrEmpty(sw))
                {
                    string temp = splitword.Replace(" ","");
                    MatchCollection m = Regex.Matches(temp, sw);
                    if (!item.ContainsKey(sw))
                    {
                        double tf = (double)m.Count / swarray.Length;
                        item.Add(sw, tf);
                    }                    
                }                
            }
            
           for (int i = 0; i < item.Count; i++)
           {
               Ew2 = Ew2 + Math.Pow(item.ElementAt(i).Value, 2);
               for (int j = 0; j < corpus.Count; j++)
               {
                   if (i == 0) Eb2 = Eb2 + Math.Pow(corpus.ElementAt(j).Value, 2);
                   if (item.ElementAt(i).Key.Equals(corpus.ElementAt(j).Key))
                   {
                       Ewb = Ewb + (item.ElementAt(i).Value * corpus.ElementAt(j).Value);
                   }
                   else continue;
               }
           }
           cosSimilarity = Ewb / (Math.Sqrt(Ew2) * Math.Sqrt(Eb2));

           return cosSimilarity;

        }

        /// <summary>
        /// 欧氏距离相似度
        /// </summary>
        /// <param name="itemName">商品名</param>
        /// <param name="Corpus">语料</param>
        /// <returns></returns>
        public static double Euclidean(string itemName, string Corpus)
        {
            double sum=0,same = 0;
            double similarity=0;
            string splitword = SplitWordHelper.RemoveSplitWord(SplitWordHelper.SplitWords(itemName));
            string[] swarray = splitword.Split(' ').Where(s => !string.IsNullOrEmpty(s)).ToArray();
            Dictionary<string, double> item = new Dictionary<string, double>();
            TFIDFJson tfidf = JsonConvert.DeserializeObject<TFIDFJson>(Corpus);
            List<KeyValuePair<string, double>> corpus = tfidf.Data;

            foreach (string sw in swarray)
            {
                if (!string.IsNullOrEmpty(sw))
                {
                    string temp = splitword.Replace(" ", "");
                    MatchCollection m = Regex.Matches(temp, sw);
                    if (!item.ContainsKey(sw))
                    {
                        double tf = (double)m.Count / swarray.Length * 100000;
                        int input = (int)tf;
                        item.Add(sw, input);
                    }
                }
            }

            for(int i = 0; i < item.Count; i++)
            {
                sum += Math.Pow(item.ElementAt(i).Value, 2);
                for(int j = 0; j < corpus.Count; j++)
                {
                    if (i == 0) sum += Math.Pow(corpus.ElementAt(j).Value, 2);
                    if (item.ElementAt(i).Key.Equals(corpus.ElementAt(j).Key))
                    {
                        sum -= (Math.Pow(item.ElementAt(i).Value, 2) + Math.Pow(corpus.ElementAt(j).Value, 2));
                        same = Math.Pow((item.ElementAt(i).Value - corpus.ElementAt(j).Value), 2);
                        sum += same;
                    }
                }
            }

            similarity = Math.Sqrt(sum);
            return similarity;
        }

        /// <summary>
        /// 根据相似度返回分类结果
        /// </summary>
        /// <param name="Similarity"></param>
        /// <returns></returns>
        static string ItemClassify(List<KeyValuePair<string, double>> Similarity)
        {
            double temp = Similarity.Last().Value;
            int index = 0;
            for (int i = 0; i < Similarity.Count; i++)
            {
                if (Similarity.ElementAt(i).Value >= temp)
                {
                    temp = Similarity.ElementAt(i).Value;
                    index = i;
                }
            }
            string classifyResultEN = Similarity.ElementAt(index).Key;
            string classifyResultCN = EnToCn[classifyResultEN];
            return classifyResultCN;
        }
        
        /// <summary>
        /// 测试分类准确度
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public static double CalculateAccuracy(string FileName)
        {
            string tfidfPath = @"..\..\TF-IDF\";
            string testDataPath = @"..\..\CrawlerResultFiles\TestData\"+FileName+".txt";
            string accuracyResultPath = @"..\..\Accuracy\"+FileName+".txt";
            SortedList<string,string> tfidfList = new SortedList<string,string>();
            foreach(string fileName in TypeModel.GetTypeNameStringArray())
            {
                StreamReader readTFIDF = new StreamReader(tfidfPath + fileName + ".txt", Encoding.Default);
                string Corpus = readTFIDF.ReadToEnd();
                tfidfList.Add(fileName,Corpus);
                readTFIDF.Close();
                readTFIDF.Dispose();
            }

            StreamReader sr = new StreamReader(testDataPath, Encoding.Default);
            string readLine = sr.ReadLine();
            int correctNum = 0, testDataNum = 0;
            string classifyResult = string.Empty;
            string result = string.Empty;
            while (!sr.EndOfStream)
            {
                testDataNum++;
                string[] testData = readLine.Split('*');
                List<KeyValuePair<string,double>> csList = new List<KeyValuePair<string,double>>();
                
                for(int i = 0; i < tfidfList.Count; i++)
                {
                    double cs = CosSimilarity(testData[1], tfidfList.ElementAt(i).Value);
                    csList.Add(new KeyValuePair<string,double>(tfidfList.ElementAt(i).Key, cs));
                }
                classifyResult = ItemClassify(csList);
                if (testData[2] == classifyResult)
                {
                    result += testData[1] + "   分类正确\r\n";
                    //Console.WriteLine("分类正确");
                    correctNum++;
                }else
                {
                    result += testData[1] +  "*分类不正确*误判为："+ classifyResult + "\r\n";
                    //Console.WriteLine("分类不正确");
                }
                readLine = sr.ReadLine();
            }
            StreamWriter sw = new StreamWriter(accuracyResultPath, true, Encoding.Default);
            
            double accuracy = (double)correctNum / testDataNum;
            sw.WriteLine(result);
            sw.Write(accuracy);
            sw.Close();
            sw.Dispose();
            sr.Close();
            sr.Dispose();
            return accuracy;
        }
    }

    class TFIDFJson
    {
        public string Type { get; set; }
        public List<KeyValuePair<string, double>> Data { get; set; }
    }

}

