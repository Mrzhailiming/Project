using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    /// <summary>
    /// 两个峰值
    /// </summary>
    public class DateAndTwoFengzhi
    {
        //数据类型, uint32, uint32 保存读取一个文件的数据
        //Dictionary<UInt32, UInt32> _data = new Dictionary<uint, uint>();

        //<文件名, <行号, 值>>
        Dictionary<string, Dictionary<UInt32, UInt32>> _data = 
            new Dictionary<string,Dictionary<uint,uint>>();

        //<文件名, 峰值>
        Dictionary<string, UInt32[]> _file2peaks = new Dictionary<string, uint[]>();

        //文件目录
        string _path;

        DateAndTwoFengzhi()
        {
            _path = null;
        }
        DateAndTwoFengzhi(string path)
        {
            _path = path;
            //读取文件
            Init();
        }
        private void Init()
        {
            //<文件名, 完整路径>
            Dictionary<string, string> allFiles;

            if (!FileReader.ScanAllFiles(_path, out allFiles) || allFiles.Count <= 0) return;

            //读取所有文件的内容
            foreach (string fileFullName in allFiles.Values)
            {
                Dictionary<UInt32, string> lineDic = new Dictionary<uint, string>();
                if (!FileReader.ReadFileLines(fileFullName, out lineDic) || lineDic.Count <= 0) continue;

                //获取每一行的date, 和对应的值
                UInt32 lineNum = 0;
                foreach (string line in lineDic.Values)
                {
                    //获取一行的数值
                    UInt32 value = GetLineValue(line);
                    //放进字典
                    AddDic(fileFullName, lineNum++, value);

                    //Logger.Log(LogType.ReadLine, string.Format("{0}:{1}", fileFullName, line));
                }
            }
            
            

        }

        public UInt32[] GetPeaks(string fileName)
        {
            return CalCulPeaks(fileName);
        }
        /// <summary>
        /// 计算峰值
        /// </summary>
        /// <returns></returns>
        private UInt32[] CalCulPeaks(string fileName)
        {
            //已经计算过了
            UInt32[] retData;
            if (!_file2peaks.TryGetValue(fileName, out retData)) return retData;

            //1.找<行, 值>
            Dictionary<UInt32, UInt32> outData;
            if (!_data.TryGetValue(fileName, out outData)) return null;
           
            //2.找峰值
            retData = findPeaks(outData);

            return retData;
        }
        /// <summary>
        /// 找峰值(两个),
        /// 规则:八帧
        /// </summary>
        /// <param name="orderedDic"></param>
        private UInt32[] findPeaks(Dictionary<UInt32, UInt32> data)
        {
            //排序
            Dictionary<UInt32, UInt32> orderedDic =
                data.OrderBy(p => p.Key).ToDictionary(p => p.Key, o => o.Value);
            UInt32 peakFirst = UInt32.MinValue;
            UInt32 peakSecond = UInt32.MinValue;
            //遍历
            foreach (UInt32 value in orderedDic.Values)
            {

            }

            return null;
        }
        /// <summary>
        /// 解析每一行的数值,规则是啥
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private UInt32 GetLineValue(string line)
        {

            return UInt32.MinValue;
        }
        /// <summary>
        /// 把一个文件的所有数据加到字典里
        /// </summary>
        /// <param name="fileFullName"></param>
        /// <param name="lineNum"></param>
        /// <param name="value"></param>
        private void AddDic(string fileFullName, UInt32 lineNum, UInt32 value)
        {
            Dictionary<UInt32, UInt32> outData;
            if (!_data.TryGetValue(fileFullName, out outData))
            {
                outData = new Dictionary<uint, uint>();
                _data[fileFullName] = outData;
            }
            outData.Add(lineNum++, value);
        }
    }
}
