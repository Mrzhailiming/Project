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
        static Queue<LogData> _dataQueue;
        static public void Log(LogType type, string data)
        {
            LogData logData = new LogData()
            {
                _type = type,
                _data = data
            };
            _dataQueue.Enqueue(logData);
        }
    }
    enum LogType
    {
        Error, //错误
        ReadFile, //扫描文件
        ReadLine, //读取文件的每一行

    }
    internal struct LogData
    {
        public LogType _type;
        public string _data;
    }
}
