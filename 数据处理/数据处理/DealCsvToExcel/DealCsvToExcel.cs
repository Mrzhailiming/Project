using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

namespace Data
{
    /// <summary>
    /// 提取想要的数据
    /// Csv格式转为excel格式
    /// </summary>
    internal class DealCsvOrExcel
    {
        //文件数据
        Dictionary<UInt32, Dictionary<string, string>> _data = new Dictionary<uint,Dictionary<string,string>>();
        UInt32 globalCount = 0;
        //行数据
        //Dictionary<string, string> _lineData;
        Dictionary<UInt32, string> _lineProperties = new Dictionary<uint,string>();
        //路径
        string _path;

        public DealCsvOrExcel(string exePath = "")
        {
            if ("" == exePath)
            {
                _path = Directory.GetCurrentDirectory();
            }
            else
            {
                _path = exePath;
            }
            Begin(_path);
        }

        /// <summary>
        /// 读取文件数据
        /// </summary>
        public void Begin(string path)
        {
            Dictionary<string, string> allFiles = new Dictionary<string, string>();
            MyFileStream.ScanAllFiles(path, allFiles, ".csv");
            if (allFiles.Count <= 0) return;

            foreach (string fileFullName in allFiles.Values)
            {
                ReadOneFile(fileFullName);
            }
            GetAverageByFootWear(_data, "2cm-3mm", "Free");
            MyConsole.WriteLine("yes", ConsoleColor.Cyan);
        }
        /// <summary>
        /// 获取数据的平均值
        /// </summary>
        /// <param name="data"></param>
        /// <param name="Footwear">以此参数作为分组依据</param>
        /// <param name="UpperExtremities">当前必选“Free”</param>
        public void GetAverageByFootWear(Dictionary<UInt32, Dictionary<string, string>> data,
            string Footwear, string UpperExtremities = "Free")
        {
            Dictionary<string, Double> sumDic = new Dictionary<string, double>();
            foreach(Dictionary<string, string> line in data.Values)
            {

                if (!CheckUpperExtremities(line, UpperExtremities)
                    || !CheckFootWear(line, Footwear))
                {
                    continue;
                }
                
                string tmp;
                if (!line.TryGetValue("Sway V - total [mm/s]", out tmp))
                {
                    continue;
                }

                Double Db = Convert.ToDouble(tmp);
                Double outDb;
                if (!sumDic.TryGetValue(Footwear, out outDb))
                {
                    sumDic.Add(Footwear, 0.0);
                }
                sumDic[Footwear] += Db;
            }
            
        }
        /// <summary>
        /// 检查UpperExtremities是否匹配
        /// </summary>
        /// <param name="line"></param>
        /// <param name="UpperExtremities"></param>
        /// <returns></returns>
        private bool CheckUpperExtremities(Dictionary<string, string> line, string UpperExtremities)
        {
            string v;
            if (!line.TryGetValue("Upper Extremities", out v)
                || v != UpperExtremities)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 检查footwear是否匹配
        /// </summary>
        /// <param name="line"></param>
        /// <param name="Footwear"></param>
        /// <returns></returns>
        private bool CheckFootWear(Dictionary<string, string> line, string Footwear)
        {
            string v;
            if (!line.TryGetValue("Footwear", out v)
                || v.IndexOf(Footwear) < 0)
            {
                return false;
            }
            return true;
        }
        void GetAverage(Dictionary<UInt32, Dictionary<string, string>> data,
            string Footwear, string UpperExtremities = "free")
        {

        }



        private void ReadOneFile(string fileFullName)
        {
            bool first = true;
            Dictionary<UInt32, string> lineDic;

            MyFileStream.ReadCsvFileLines(fileFullName, out lineDic);

            foreach (string data in lineDic.Values)
            {
                //第一行是字段
                if (first)
                {
                    UInt32 count = 0;
                    string[] strArr = data.Split(',');
                    foreach (string pro in strArr)
                    {
                        _lineProperties[count++] = pro;
                    }

                    first = false;
                }
                //第二行开始是数据
                else
                {
                    UInt32 count = 0;
                    Dictionary<string, string> tmp = new Dictionary<string, string>();
                    string[] strArr = data.Split(',');
                    foreach (string pro in strArr)
                    {
                        tmp[_lineProperties[count++]] = pro;
                    }
                    _data[globalCount++] = tmp;
                }

            }
        }



    }
}
