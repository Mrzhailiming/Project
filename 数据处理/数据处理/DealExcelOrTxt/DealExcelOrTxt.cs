using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.IO;

namespace Data
{
    /// <summary>
    /// 从Excel或txt中找出特定行
    /// </summary>
    class DealExcelOrTxt
    {
        public Dictionary<string, string> _allFlies = new Dictionary<string, string>();
        Dictionary<string, Dictionary<uint, string>> _data = new Dictionary<string, Dictionary<uint, string>>();

        string _filePath;
         public DealExcelOrTxt(string filePath)
        {
            if (filePath == "")
            {
                filePath = Directory.GetCurrentDirectory();
            }

            _filePath = filePath;
            Init();
        }
        void Init()
        {
            
            ScanFiles(_filePath, _allFlies);

            try
            {
                foreach (string fileFullName in _allFlies.Values)
                {
                    Dictionary<uint, string> fileLineData;
                    ReadFile(fileFullName, out fileLineData);

                    _data.Add(fileFullName, fileLineData);

                }
            }
            catch (Exception ex)
            {
                Logger.Instance().Log(LogType.Error, ex.ToString());
            }
            

        }
        /// <summary>
        /// 扫描文件
        /// </summary>
        /// <param name="allFiles"></param>
        void ScanFiles(string path, Dictionary<string, string> allFiles)
        {
            MyFileStream.ScanAllFiles(path, allFiles, ".xls");
            //MyFileStream.ScanAllFiles(path, allFiles, ".txt");
        }
        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="data"></param>
        void ReadFile(string fileFullName, out Dictionary<uint, string> fileLineData)
        {
            MyFileStream.ReadTxtFileLines(fileFullName, out fileLineData);
        }
        /// <summary>
        /// 根据前置行，找出特定后缀的特定行
        /// </summary>
        /// <param name="preLine">前置行</param>
        /// <param name="outAllFilesLineData">文件名，数据</param>
        /// <param name="filePostName">特定文件后缀名</param>
        public void FindAllFilesLineByPreLine(string[] preLine, out Dictionary<string, string> outAllFilesLineData,
            string filePostName)
        {
            outAllFilesLineData = new Dictionary<string, string>();

            foreach (KeyValuePair<string, Dictionary<uint, string>> oneFileData in _data)
            {
                //找特定文件后缀
                //1cm-5mm_3 - 2021-5-10 - Foot Dimensions.xls
                if (oneFileData.Key.ToLower().IndexOf(filePostName.ToLower()) < 0)
                {
                    continue;
                }

                //找特定行
                string outLineData;
                FindLineByPreLine(oneFileData, preLine, out outLineData);

                
                //输出到文件 FindLineByPreLine.txt
                OutPutToFile(oneFileData.Key, outLineData, "FindLineByPreLine");

                //
                outAllFilesLineData.Add(oneFileData.Key, outLineData);
            }
            
        }
        /// <summary>
        /// 根据前置行查找多个特定行
        /// </summary>
        /// <param name="preLine"></param>
        /// <param name="outAllFilesLineData">文件名，多行数据</param>
        /// <param name="filePostName">特定后缀名</param>
        public void FindMultiLine(string[] preLine, 
            out Dictionary<string, Dictionary<uint, string>> outAllFilesLineData,
            string filePostName)
        {
            //1cm-5mm_3 - 2021-5-10 - Foot Dimensions.xls
            outAllFilesLineData = new Dictionary<string, Dictionary<uint, string>>();
            foreach (KeyValuePair<string, Dictionary<uint, string>> oneFileData in _data)
            {
                //找特定文件后缀
                //1cm-5mm_3 - 2021-5-10 - Foot Dimensions.xls
                if (oneFileData.Key.ToLower().IndexOf(filePostName.ToLower()) < 0)
                {
                    continue;
                }

                //找特定行
                Dictionary<uint, string> outMultiLineData;
                FindMultiLineByPreLine(oneFileData, preLine, out outMultiLineData);


                //输出到文件 FindLineByPreLine.txt
                OutPutToFile(oneFileData.Key, outMultiLineData, "FindMultiLineByPreLine");

                //
                outAllFilesLineData.Add(oneFileData.Key, outMultiLineData);
            }

        }
        /// <summary>
        /// 输出记录
        /// </summary>
        /// <param name="fileFullName">处理的文件名</param>
        /// <param name="lineData">行数据</param>
        /// <param name="outputFileName">输出的文件名</param>
        void OutPutToFile(string fileFullName, string lineData, string outputFileName = "default")
        {
            MyFileStream.WriteFile(_filePath + "\\数据", outputFileName,
                    string.Format("{0} {1}", fileFullName, lineData));
        }
        void OutPutToFile(string fileFullName, Dictionary<uint, string> MultilineData, string outputFileName = "default")
        {
            string data = "";
            foreach (string line in MultilineData.Values)
            {
                data = data + line + "\r\n";
            }
            MyFileStream.WriteFile(_filePath + "\\数据", outputFileName + "\r\n",
                    string.Format("{0} {1}", fileFullName, data));
        }
        /// <summary>
        /// 通过前置行，找到本行
        /// </summary>
        /// <param name="preLine">多个前置行，都要匹配</param>
        /// <param name="outLineData"></param>
        private void FindLineByPreLine(KeyValuePair<string, Dictionary<uint, string>> allLines, 
            string[] preLine, out string outLineData)
        {
            outLineData = "";
            int length = preLine.Length;
            //按位操作？
            int map = 0;
            int index = 0;
            foreach (string line in allLines.Value.Values)
            {
                if (CheckMap(map, length))
                {
                    outLineData = line;
                    break;
                }

                //忽略大小写
                if (index < length && line.ToLower().IndexOf(preLine[index].ToLower()) == 0)
                {
                    map = map | (1 << index);
                    ++index;
                }
            }

            if (outLineData == "")
            {
                string log = allLines.Key + "--" + preLine + "--" + "!!!not found!!!";
                //记录日志
                Logger.Instance().Log(LogType.FindLineByPreLineNotFound, log);
            }
        }
        void FindMultiLineByPreLine(KeyValuePair<string, Dictionary<uint, string>> allLines,
            string[] preLine, out Dictionary<uint, string> outMultiLineData)
        {
            outMultiLineData = new Dictionary<uint, string>();
            uint count = 0;

            //按位操作
            int length = preLine.Length;
            int map = 0;
            int index = 0;
            //根据前置行找到特定行
            foreach (string line in allLines.Value.Values)
            {
                if (CheckMap(map, length))
                {

                    if ("" == line)
                    {
                        break;
                    }

                    outMultiLineData.Add(++count, line);
                    continue;
                }

                //忽略大小写
                if (index < length 
                    && line.ToLower().IndexOf(preLine[index].ToLower()) == 0)
                {
                    map = map | (1 << index);
                    ++index;
                }
            }
        }
        //按位操作
        bool CheckMap(int map, int length)
        {
            for(int i = 0; i < length; ++i)
            {
                if (((map >> i) & 1) == 0)
                {
                    return false;
                }

            }
            return true;
        }
    }
}
