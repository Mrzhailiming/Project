using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace Data
{
    /// <summary>
    /// 文件相关
    /// </summary>
    internal class FileReader
    {
        /// <summary>
        /// 扫描目录下所有文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="allFiles"><文件名, 完整路径></param>
        /// <returns></returns>
        public static bool ScanAllFiles(string path, out Dictionary<string, string> allFiles)
        {
            allFiles = new Dictionary<string,string>();
            if (null == path) return false;
            try
            {
                DirectoryInfo root = new DirectoryInfo(path);
                FileInfo[] files = root.GetFiles();

                foreach (FileInfo fileInfo in files)
                {
                    allFiles[fileInfo.Name] = fileInfo.FullName;
                    //记录日志
                    Logger.Log(LogType.ScanFile, fileInfo.FullName);
                }
                return true;
            }
            catch (Exception ex)
            {
                //记录异常日志
                Logger.Log(LogType.Error, ex.ToString());
            }

            return false;
        }
        /// <summary>
        /// 读一个文件的所有行
        /// </summary>
        /// <param name="fileFullPath">文件完整路径</param>
        /// <param name="lineDic"><行,line></param>
        /// <returns></returns>
        public static bool ReadFileLines(string fileFullPath, out Dictionary<UInt32, string> lineDic)
        {
            lineDic = null;
            try
            {
                using (StreamReader reader = new StreamReader(fileFullPath))
                {
                    string line = null;
                    UInt32 count = 0;
                    while ((line = reader.ReadLine()) != null)
                    {
                        lineDic[count++] = line;
                        //记录日志
                        Logger.Log(LogType.ReadLine, line);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                //异常日志
                Logger.Log(LogType.Error, ex.ToString());
            }
            return false;
        }
        //写
        public static bool WriteFile(string fileFullPath, out Dictionary<UInt32, string> lineDic)
        {
            lineDic = null;
            return false;
        }
    }
}
