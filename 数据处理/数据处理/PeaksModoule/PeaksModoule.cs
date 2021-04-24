using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class PeaksModoule
    {

        //<文件名, 完整路径>
        public Dictionary<string, string> _allFiles = new Dictionary<string, string>();

        //<文件名, <行号, 值>>
        public Dictionary<string, Dictionary<UInt32, UInt32>> _data = 
            new Dictionary<string,Dictionary<uint,uint>>();

        //<文件完整路径, 峰值>
        public Dictionary<string, UInt32[]> _file2peaks = new Dictionary<string, uint[]>();

        //文件目录
        public string _path;

        //有效数据的起始值
        private UInt32 _beginValue = (UInt32)PeakValue.PeakMinValue;

        public PeaksModoule()
        {
            _path = null;
        }
        public PeaksModoule(string path)
        {
            _path = path;
            //读取文件
            Init();
        }
        /// <summary>
        /// 初始化
        /// 1。扫描所有文件
        /// 2。读取文件所有内容
        /// </summary>
        private void Init()
        {

            if (!MyFileStream.ScanAllFiles(_path, out _allFiles) || _allFiles.Count <= 0) return;

            //读取所有文件的内容
            foreach (string fileFullName in _allFiles.Values)
            {
                Dictionary<UInt32, string> lineDic;
                if (!MyFileStream.ReadFileLines(fileFullName, out lineDic) || lineDic.Count <= 0) continue;

                //获取每一行的date, 和对应的值
                foreach (string line in lineDic.Values)
                {
                    //获取一行的数值
                    UInt32 date = GetLineDate(line);
                    UInt32 value = GetLineValue(line);
                    
                    //放进字典
                    AddDic(fileFullName, date, value);
                }
            }
            
        }
        /// <summary>
        /// 获取所有文件
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetAllFiles()
        {
            return _allFiles;
        }
        /// <summary>
        /// 获取文件的峰值
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public UInt32[] GetPeaks(string fileName)
        {
            return CalCulPeaks(_allFiles[fileName]);
        }
        /// <summary>
        /// 计算峰值
        /// </summary>
        /// <returns></returns>
        private UInt32[] CalCulPeaks(string fileFullName)
        {
            //已经计算过了
            UInt32[] retData;

            //1.有无这个文件
            Dictionary<UInt32, UInt32> outData;
            if (!_data.TryGetValue(fileFullName, out outData)) return null;

            //2.峰值缓存
            if (_file2peaks.TryGetValue(fileFullName, out retData)) return retData;
           
            //2.找峰值
            retData = findPeaks(outData);
            _file2peaks[fileFullName] = retData;

            return retData;
        }
        /// <summary>
        /// 在数据中查找峰值，(子类重写)
        /// </summary>
        /// <param name="orderedDic"></param>
        virtual public UInt32[] findPeaks(Dictionary<UInt32, UInt32> data)
        {
            UInt32[] ret = new UInt32[2];
            //排序
            Dictionary<UInt32, UInt32> orderedDic =
                data.OrderBy(p => p.Key).ToDictionary(p => p.Key, o => o.Value);
            UInt32 peakFirst = _beginValue;
            UInt32 peakSecond = _beginValue;
            UInt32 peakThree = _beginValue;
            bool first = false;//第一个峰值的标志
            bool second = false;//峰谷
            bool third = false;//第二个峰值
            //遍历
            foreach (UInt32 value in orderedDic.Values)
            {
                //小于10的数据不要
                if (value <= 10)
                {
                    continue;
                }
                //第一个峰值
                if (!first)
                {
                    if (value > peakFirst)
                    {
                        peakFirst = value;
                    }
                    else//找到
                    {
                        peakSecond = value;
                        first = true;
                    }
                }
                //峰谷
                else if (!second)
                {
                    if (value < peakSecond)
                    {
                        peakSecond = value;
                    }
                    else//找到
                    {
                        second = true;
                    }
                }
                //第二个峰值
                else if(!third)
                {
                    if (value > peakThree)
                    {
                        peakThree = value;
                    }
                    else
                    {
                        third = true;
                    }
                }
                else
                {
                    break;
                }
            }
            //
            ret[0] = peakFirst;
            ret[1] = peakThree;

            return ret;
        }
        /// <summary>
        /// 解析每一行的数值,（根据规则解析，子类重写）
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        virtual public UInt32 GetLineValue(string line)
        {
            try
            {
                string value = line.Substring(line.LastIndexOf(" ") + 1);
                return Convert.ToUInt32(value);
            }
            catch (Exception ex)
            {
                Logger.Instance().Log(LogType.JieXiFailed, string.Format("GetLineValue error line:{0}", line));
            }
            return UInt32.MinValue;
        }
        /// <summary>
        /// 解析每一行的时间,（根据规则解析，子类重写）
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private UInt32 GetLineDate(string line)
        {
            try
            {
                int end = line.LastIndexOf(" ");
                string value = line.Substring(0, end);
                return Convert.ToUInt32(value);
            }
            catch (Exception ex)
            {
                Logger.Instance().Log(LogType.JieXiFailed, string.Format("GetLineDate error line:{0}", line));
            }
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
    /// <summary>
    /// 有效数据的起始值
    /// </summary>
    public enum PeakValue
    {
        PeakMinValue = 10, //峰值的最小值
    }
}
