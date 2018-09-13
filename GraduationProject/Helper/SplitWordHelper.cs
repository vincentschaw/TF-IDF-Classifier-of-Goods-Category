using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.IO;

namespace GraduationProject
{
    [StructLayout(LayoutKind.Explicit)]
    public struct result_t
    {
        [FieldOffset(0)]
        public int start;
        [FieldOffset(4)]
        public int length;
        [FieldOffset(8)]
        public int sPos1;
        [FieldOffset(12)]
        public int sPos2;
        [FieldOffset(16)]
        public int sPos3;
        [FieldOffset(20)]
        public int sPos4;
        [FieldOffset(24)]
        public int sPos5;
        [FieldOffset(28)]
        public int sPos6;
        [FieldOffset(32)]
        public int sPos7;
        [FieldOffset(36)]
        public int sPos8;
        [FieldOffset(40)]
        public int sPos9;
        [FieldOffset(44)]
        public int sPos10;
        //[FieldOffset(12)] public int sPosLow;
        [FieldOffset(48)]
        public int POS_id;
        [FieldOffset(52)]
        public int word_ID;
        [FieldOffset(56)]
        public int word_type;
        [FieldOffset(60)]
        public int weight;
    }
    public static class SplitWordHelper
    {
        #region dll初始化
        const string path = @"..\..\hanyufenci\lib\win64\NLPIR.dll";//设定dll的路径
        //对函数进行申明
        [DllImport(path, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi, EntryPoint = "NLPIR_Init")]
        public static extern bool NLPIR_Init(String sInitDirPath, int encoding, String sLicenseCode);

        //特别注意，C语言的函数NLPIR_API const char * NLPIR_ParagraphProcess(const char *sParagraph,int bPOStagged=1);必须对应下面的申明
        [DllImport(path, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi, EntryPoint = "NLPIR_ParagraphProcess")]
        public static extern IntPtr NLPIR_ParagraphProcess(String sParagraph, int bPOStagged = 1);

        [DllImport(path, CharSet = CharSet.Ansi, EntryPoint = "NLPIR_Exit")]
        public static extern bool NLPIR_Exit();

        [DllImport(path, CharSet = CharSet.Ansi, EntryPoint = "NLPIR_ImportUserDict")]
        public static extern int NLPIR_ImportUserDict(String sFilename);

        [DllImport(path, CharSet = CharSet.Ansi, EntryPoint = "NLPIR_FileProcess")]
        public static extern bool NLPIR_FileProcess(String sSrcFilename, String sDestFilename, int bPOStagged = 1);

        [DllImport(path, CharSet = CharSet.Ansi, EntryPoint = "NLPIR_FileProcessEx")]
        public static extern bool NLPIR_FileProcessEx(String sSrcFilename, String sDestFilename);

        [DllImport(path, CharSet = CharSet.Ansi, EntryPoint = "NLPIR_GetParagraphProcessAWordCount")]
        static extern int NLPIR_GetParagraphProcessAWordCount(String sParagraph);
        //NLPIR_GetParagraphProcessAWordCount
        [DllImport(path, CharSet = CharSet.Ansi, EntryPoint = "NLPIR_ParagraphProcessAW")]
        static extern void NLPIR_ParagraphProcessAW(int nCount, [Out, MarshalAs(UnmanagedType.LPArray)] result_t[] result);

        [DllImport(path, CharSet = CharSet.Ansi, EntryPoint = "NLPIR_AddUserWord")]
        static extern int NLPIR_AddUserWord(String sWord);

        [DllImport(path, CharSet = CharSet.Ansi, EntryPoint = "NLPIR_SaveTheUsrDic")]
        static extern int NLPIR_SaveTheUsrDic();

        [DllImport(path, CharSet = CharSet.Ansi, EntryPoint = "NLPIR_DelUsrWord")]
        static extern int NLPIR_DelUsrWord(String sWord);

        [DllImport(path, CharSet = CharSet.Ansi, EntryPoint = "NLPIR_NWI_Start")]
        static extern bool NLPIR_NWI_Start();

        [DllImport(path, CharSet = CharSet.Ansi, EntryPoint = "NLPIR_NWI_Complete")]
        static extern bool NLPIR_NWI_Complete();

        [DllImport(path, CharSet = CharSet.Ansi, EntryPoint = "NLPIR_NWI_AddFile")]
        static extern bool NLPIR_NWI_AddFile(String sText);

        [DllImport(path, CharSet = CharSet.Ansi, EntryPoint = "NLPIR_NWI_AddMem")]
        static extern bool NLPIR_NWI_AddMem(String sText);

        [DllImport(path, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi, EntryPoint = "NLPIR_NWI_GetResult")]
        public static extern IntPtr NLPIR_NWI_GetResult(bool bWeightOut = false);

        [DllImport(path, CharSet = CharSet.Ansi, EntryPoint = "NLPIR_NWI_Result2UserDict")]
        static extern uint NLPIR_NWI_Result2UserDict();

        [DllImport(path, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi, EntryPoint = "NLPIR_GetKeyWords")]
        public static extern IntPtr NLPIR_GetKeyWords(String sText, int nMaxKeyLimit = 50, bool bWeightOut = false);

        [DllImport(path, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Winapi, EntryPoint = "NLPIR_GetFileKeyWords")]
        public static extern IntPtr NLPIR_GetFileKeyWords(String sFilename, int nMaxKeyLimit = 50, bool bWeightOut = false);
        #endregion

        static SplitWordHelper()
        {
            string filepath = @"..\..\hanyufenci\";
            NLPIR_ImportUserDict(filepath + @"Data\userdic.txt");
            //NLPIR_SaveTheUsrDic();
            NLPIR_Init(filepath, 0, "");
        }

        /// <summary>
        /// 初步分词
        /// </summary>
        /// <param name="itemName">商品名</param>
        /// <returns></returns>
        public static string SplitWords(string itemName)
        {
            int count = NLPIR_GetParagraphProcessAWordCount(itemName);//先得到结果的词数

            result_t[] result = new result_t[count];//在客户端申请资源
            NLPIR_ParagraphProcessAW(count, result);//获取结果存到客户的内存中
            
            StringBuilder sResult = new StringBuilder(600);
            //准备存储空间         

            IntPtr intPtr = NLPIR_ParagraphProcess(itemName);//切分结果保存为IntPtr类型
            String str = Marshal.PtrToStringAnsi(intPtr);//将切分结果转换为string

            return str;
        }

        /// <summary>
        /// 按照规则剔除分词结果中不需要的词并返回新的分词结果
        /// </summary>
        /// <param name="splitword">分词结果</param>
        /// <returns></returns>
        static public string RemoveSplitWord(string splitword)
        {
            if (splitword.Contains('+'))
            {
                splitword = splitword.Replace("+", "");
            }
            string[] array = splitword.Split(' ');
            string result = string.Empty;
            foreach (string str in array)
            {
                Match m = Regex.Match(str.Split('/')[0], @"([a-z])|([A-Z])");
                Match m2 = Regex.Match(str.Split('/')[0].ToLower(), "(t恤)|(polo)");
                bool rule = str.Contains("/m") || str.Contains("/q") || str.Contains("/x") || str.Contains("/nrf") || str.Contains("/w")
                    || str.Contains("/rzv") || str.Contains("/nr") || str.Contains("/a") || str.Contains("/d") || str.Contains("/v")
                    || str.Contains("/u") || str.Contains("2018") || str.Contains("2017") || (m.Captures.Count > 0 && m2.Captures.Count == 0);

                bool rule2 = str.Contains("/m") || str.Contains("/q") || str.Contains("/x") || str.Contains("/nrf") || str.Contains("/w")
                    || str.Contains("/rzv") || str.Contains("/nr") || str.Contains("/d") || str.Contains("/a")
                    || str.Contains("/u") || str.Contains("2018") || str.Contains("2017") || (m.Captures.Count > 0 && m2.Captures.Count == 0);

                bool rule3 = !str.Contains("/n") || (m.Captures.Count > 0 && m2.Captures.Count == 0) || str.Contains("2018") || str.Contains("2017");

                bool rule4 = str.Contains("/m") || str.Contains("/q") || str.Contains("/x") || str.Contains("/nrf") || str.Contains("/w")
                    || str.Contains("/rzv") || str.Contains("/nr") || str.Contains("/d") || str.Contains("/a")
                    || str.Contains("/u") || str.Contains("2018") || str.Contains("2017") || str.Contains("/y") || str.Contains("/nz") || (m.Captures.Count > 0 && m2.Captures.Count == 0);
                if (rule4)
                {
                    continue;
                }
                else
                {
                    result += str.ToLower() + " ";
                }
            }
            return result;
        }

        /// <summary>
        /// 获取处理后的分词结果并保存到文本文件中
        /// </summary>
        static public void GetSplitWordResultFiles()
        {
            string srpath = @"..\..\CrawlerResultFiles\Taobao\";
            string swpath = srpath + @"..\..\SplitWordResultFiles\";
            try
            {
                foreach (string file in TypeModel.GetTypeNameStringArray())
                {
                    Console.WriteLine("正在处理" + file + "分词结果");
                    StreamReader sr = new StreamReader(srpath + file + ".txt", Encoding.Default);
                    StreamWriter sw = new StreamWriter(swpath + file + ".txt", true, Encoding.Default);
                    string readLine = sr.ReadLine();
                    while (!sr.EndOfStream)
                    {
                        string[] rawdata = readLine.Split('*');
                        string itemName = rawdata[1];                        
                        string splitword = SplitWords(itemName);
                        string result = RemoveSplitWord(splitword);
                        sw.WriteLine(result);
                        readLine = sr.ReadLine();
                    }
                    sr.Close();
                    sr.Dispose();
                    sw.Close();
                    sw.Dispose();
                }
                Console.WriteLine("处理完成");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }

    }
}
