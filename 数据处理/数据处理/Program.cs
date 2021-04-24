using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace Data
{
    class Program
    {
        static void Main(string[] args)
        {
            //获取程序当前路径
            string path = Directory.GetCurrentDirectory();
            DateAndTwoFengzhi d = new DateAndTwoFengzhi(path);

            Dictionary<string, string> allFiles = d.GetAllFiles();

            foreach (string fileName in allFiles.Keys)
            {
                UInt32[] peaks = d.GetPeaks(fileName);
                Console.WriteLine("文件:{0}\t峰值1:{1} 峰值2:{2}", fileName, peaks[0], peaks[1]);
                MyFileStream.WriteFile(path + "\\数据", "peaks", 
                    string.Format("{0} {1}\t{2}", peaks[0], peaks[1], fileName));
            }

            Console.ReadKey();
        }
    }
}
