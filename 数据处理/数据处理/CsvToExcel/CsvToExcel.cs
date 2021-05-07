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
    /// </summary>
    internal class CsvToExcel
    {
        //文件数据
        Dictionary<UInt32, Dictionary<string, string>> _data = new Dictionary<uint,Dictionary<string,string>>();
        UInt32 globalCount = 0;
        //行数据
        //Dictionary<string, string> _lineData;
        Dictionary<UInt32, string> _lineProperties = new Dictionary<uint,string>();
        //路径
        string _path;

        CsvToExcel()
        {
            _path = Directory.GetCurrentDirectory();
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
