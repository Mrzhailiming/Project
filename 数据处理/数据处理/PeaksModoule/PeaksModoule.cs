using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class PeaksModoule
    {

        //<文件完整路径, 完整路径>
        public Dictionary<string, string> _allFiles = new Dictionary<string, string>();

        //<文件完整路径, <行号, 值>>
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

            
            foreach (string fileFullName in _allFiles.Values)
            {
                if (!ReadAndDecodeFile(fileFullName))
                {
                    //读取文件失败
                    _allFiles.Remove(fileFullName);
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
        /// 检查文件是否在字典里,不在就加上
        /// </summary>
        /// <param name="fileFullName"></param>
        private bool CheckFile(string fileFullName)
        {
            string file;
            if (!_allFiles.TryGetValue(fileFullName, out file))
            {
                //不在扫到的文件内
                _allFiles[fileFullName] = fileFullName;
            }
            return true;
        }
        /// <summary>
        /// 检查是否读取文件的内容，没读就读取
        /// </summary>
        /// <param name="fileFullName"></param>
        private bool CheckFileData(string fileFullName, out Dictionary<UInt32, UInt32> fileData)
        {
            if (!_data.TryGetValue(fileFullName, out fileData))
            {
                //读取文件的内容
                if (!ReadAndDecodeFile(fileFullName))
                {
                    //读取失败
                    _allFiles.Remove(fileFullName);
                    return false;
                }
                _data.TryGetValue(fileFullName, out fileData);
            }
            return true;
        }
        /// <summary>
        /// 检查是否已经计算了文件的峰值，没计算就计算
        /// </summary>
        /// <param name="fileFullName"></param>
        private bool CheckFilePeaks(string fileFullName, out UInt32[] peaksData)
        {
            if (!_file2peaks.TryGetValue(fileFullName, out peaksData))
            {
                Dictionary<UInt32, UInt32> outData;
                _data.TryGetValue(fileFullName, out outData);

                CalCulPeaks(_allFiles[fileFullName], outData, out peaksData);

                _file2peaks.TryGetValue(fileFullName, out peaksData);
            }
            return true;
        }
        /// <summary>
        /// 获取文件的峰值
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public UInt32[] GetPeaks(string fileFullName)
        {
            CheckFile(fileFullName);

            Dictionary<UInt32, UInt32> fileData;
            CheckFileData(fileFullName, out fileData);

            UInt32[] retData;
            CheckFilePeaks(fileFullName, out retData);

            return retData;
            
        }
        /// <summary>
        /// 重新计算文件的峰值
        /// </summary>
        /// <param name="fileFullName"></param>
        /// <returns></returns>
        public UInt32[] ReGetPeaks(string fileFullName)
        {
            CheckFile(fileFullName);

            Dictionary<UInt32, UInt32> fileData;
            CheckFileData(fileFullName, out fileData);

            UInt32[] retData;
            //计算峰值
            CalCulPeaks(_allFiles[fileFullName], fileData, out retData);
            
            return retData;

        }
        /// <summary>
        /// 检查文件名合法性
        /// ".txt" 只能有一个
        /// </summary>
        private bool CheckFileName(string fileFullName)
        {
            int index = fileFullName.LastIndexOf(".");
            if (index < 0)
            {
                return false;
            }
            string extName = fileFullName.Substring(index);
            if (extName != ".txt")
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 计算峰值
        /// </summary>
        /// <returns></returns>
        private void CalCulPeaks(string fileFullName, Dictionary<UInt32, UInt32> data, out UInt32[] retData)
        {
            retData = findPeaks(data);
            _file2peaks[fileFullName] = retData;

        }
        /// <summary>
        /// 在数据中查找峰值，(子类重写)
        /// </summary>
        /// <param name="orderedDic"></param>
        virtual public UInt32[] findPeaks(Dictionary<UInt32, UInt32> data)
        {
            UInt32[] ret = new UInt32[2] { 0, 0 };
            //排序
            try
            {
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
                    else if (!third)
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
            catch (Exception ex)
            {
                //异常日志
                Logger.Instance().Log(LogType.Error, ex.ToString());
            }
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
        /// <summary>
        /// 读取一个文件的所有行，并解析
        /// </summary>
        /// <param name="fileFullName"></param>
        private bool ReadAndDecodeFile(string fileFullName)
        {
            Dictionary<UInt32, string> lineDic;
            //读取文件的内容
            if (!MyFileStream.ReadFileLines(fileFullName, out lineDic) || lineDic.Count <= 0)
            {
                return false;
            }

            //获取每一行的date, 和对应的值
            foreach (string line in lineDic.Values)
            {
                //获取一行的数值
                UInt32 date = GetLineDate(line);
                UInt32 value = GetLineValue(line);

                //放进字典
                AddDic(fileFullName, date, value);
            }
            return true;
        }
        virtual public void Reset()
        {
            //清除这个，重新打印起始行
            Logger._hadLogFiles.Clear();
            //<文件完整路径, 完整路径>
            _allFiles.Clear();

            //<文件完整路径, <行号, 值>>
            _data.Clear();

            //<文件完整路径, 峰值>
            _file2peaks.Clear();

            //文件目录
            _path = "";
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
