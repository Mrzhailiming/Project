﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.IO;

namespace Data
{
    
    /// <summary>
    /// 打印日志
    /// </summary>
    public class Logger : Singletion<Logger>
    {
        static Queue<LogData> _dataQueue = new Queue<LogData>();
        static string _logFileFullPath = null;
        string _logFileName = null;
        public Logger()
        {
            _logFileFullPath = string.Format("{0}\\log",Directory.GetCurrentDirectory());
            Thread t = new Thread(Execute);
            t.Start();
        }
        Logger(string logFileFullPath, string logFileName)
        {
            //_logFileFullPath = logFileFullPath;
            //_logFileName = logFileName;
            //Thread t = new Thread(Execute);
            //t.Start();
        }
        public void Log(LogType type, string data)
        {
            LogData logData = new LogData()
            {
                _type = type,
                _data = data
            };
            lock (_dataQueue)
            {
                _dataQueue.Enqueue(logData);
            }
        }
        virtual public void Execute()
        {
            while (true)
            {
                LogData data;
                lock (_dataQueue)
                {
                    if (_dataQueue.Count > 0)
                    {
                        lock (_dataQueue)
                        {
                            data = _dataQueue.Dequeue();

                        }
                        MyFileStream.WriteFile(_logFileFullPath, data._type.ToString(), data._data);
                    }
                }
                
            }
        }
    }
    public enum LogType
    {
        Error, //错误
        ScanFile, //扫描文件
        ReadLine, //读取文件的每一行
        JieXiFailed, //解析行失败
        WriteFileFailed,//写入文件失败

    }
    internal struct LogData
    {
        public LogType _type;
        public string _data;
    }
}
