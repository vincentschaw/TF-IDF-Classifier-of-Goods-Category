using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;



namespace GraduationProject
{
    class Program
    {
        static void Main(string[] args)
        {
            //对训练文本进行分词处理
            SplitWordHelper.GetSplitWordResultFiles();

            //对分词后的文本进行特征提取
            CommonHelper.TFIDF();

            //计算每个商品分类的分类结果
            double totalAccuracy = 0;
            foreach (string fileName in TypeModel.GetTypeNameStringArray())
            {
                double accuracy = CommonHelper.CalculateAccuracy(fileName);
                Console.WriteLine(fileName + "'s accuracy: " + accuracy);
                totalAccuracy += accuracy;
            }
            Console.WriteLine("总准确率为" + (totalAccuracy / 16).ToString());

            Console.ReadKey();
        }
    }


}
