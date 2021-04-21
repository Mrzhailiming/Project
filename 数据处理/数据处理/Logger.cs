using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    
    /// <summary>
    /// 打印日志
    /// </summary>
    public class Logger
    {
        static Queue<LogData> _dataQueue = new Queue<LogData>();
        public static void Log(LogType type, string data)
        {
            LogData logData = new LogData()
            {
                _type = type,
                _data = data
            };
            _dataQueue.Enqueue(logData);
        }
    }
    public enum LogType
    {
        Error, //错误
        ScanFile, //扫描文件
        ReadLine, //读取文件的每一行

    }
    internal struct LogData
    {
        public LogType _type;
        public string _data;
    }
}
