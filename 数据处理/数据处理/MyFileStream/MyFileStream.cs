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
    internal class MyFileStream
    {
        /// <summary>
        /// 扫描目录下所有文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="allFiles"><文件名, 完整路径></param>
        /// <returns></returns>
        public static Dictionary<string, string> ScanAllFiles(string path, Dictionary<string, string> allFiles, string ext = ".txt")
        {
            if (null == path) return allFiles;
            try
            {
                DirectoryInfo root = new DirectoryInfo(path);
                FileInfo[] files = root.GetFiles();

                foreach (FileInfo fileInfo in files)
                {
                    string fileName = fileInfo.Name;
                    string exname = fileName.Substring(fileName.LastIndexOf(".") + 1);//得到后缀名
                    if (ext.IndexOf(fileName.Substring(fileName.LastIndexOf(".") + 1)) > -1)//如果后缀名为.txt文件
                    {
                        allFiles[fileInfo.FullName] = fileInfo.FullName;
                        //记录日志
                        Logger.Instance().Log(LogType.ScanFile, fileInfo.FullName);
                    }
                    
                }
                return allFiles;
            }
            catch (Exception ex)
            {
                //记录异常日志
                Logger.Instance().Log(LogType.Error, ex.ToString());
            }

            return allFiles;
        }
        /// <summary>
        /// 读一个txt文件的所有行
        /// </summary>
        /// <param name="fileFullPath">文件完整路径</param>
        /// <param name="lineDic"><行,line></param>
        /// <returns></returns>
        public static bool ReadTxtFileLines(string fileFullPath, out Dictionary<UInt32, string> lineDic)
        {
            lineDic = new Dictionary<uint, string>();
            try
            {
                //检查文件是否存在
                if (!File.Exists(fileFullPath))
                {
                    MyConsole.WriteLine("文件不存在或没有以管理员身份运行或文件名输入不正确，不要带引号", ConsoleColor.Red);
                    Logger.Instance().Log(LogType.FileNotExist, "文件名：" + fileFullPath);
                    return false;
                }
                using (StreamReader reader = new StreamReader(fileFullPath))
                {
                    string line = null;
                    UInt32 count = 0;
                    //首行
                    Logger.Instance().Log(LogType.ReadLineTxt, string.Format("<-{0}:{1}->", fileFullPath, DateTime.Now.ToString()));
                    while ((line = reader.ReadLine()) != null)
                    {
                        lineDic[count++] = line;
                        //记录日志
                        Logger.Instance().Log(LogType.ReadLineTxt, string.Format("{0}:{1}", fileFullPath, line));
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                //异常日志
                Logger.Instance().Log(LogType.Error, ex.ToString());
            }
            return false;
        }
        /// <summary>
        /// 读一个CSV文件的所有行
        /// </summary>
        /// <param name="fileFullPath">文件完整路径</param>
        /// <param name="lineDic"><行,line></param>
        /// <returns></returns>
        public static bool ReadCsvFileLines(string fileFullPath, out Dictionary<UInt32, string> lineDic)
        {
            lineDic = new Dictionary<uint, string>();
            try
            {
                //检查文件是否存在
                if (!File.Exists(fileFullPath))
                {
                    MyConsole.WriteLine("文件不存在或没有以管理员身份运行或文件名输入不正确，不要带引号", ConsoleColor.Red);
                    Logger.Instance().Log(LogType.FileNotExist, "文件名：" + fileFullPath);
                    return false;
                }
                using (StreamReader reader = new StreamReader(fileFullPath))
                {
                    string line = null;
                    UInt32 count = 0;
                    //首行
                    Logger.Instance().Log(LogType.ReadLineCsv, string.Format("<-{0}:{1}->", fileFullPath, DateTime.Now.ToString()));
                    while ((line = reader.ReadLine()) != null)
                    {
                        lineDic[count++] = line;
                        //记录日志
                        Logger.Instance().Log(LogType.ReadLineCsv, string.Format("{0}:{1}", fileFullPath, line));
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                //异常日志
                Logger.Instance().Log(LogType.Error, ex.ToString());
            }
            return false;
        }
        //写
        public static bool WriteFile(string fileFullPath, out Dictionary<UInt32, string> lineDic)
        {
            lineDic = null;
            return false;
        }
        /// <summary>
        /// 写（缺陷，每调用一次就要打开一次文件）
        /// </summary>
        /// <param name="fileFullPath">完整路径</param>
        /// <param name="lineDic">文件名</param>
        /// <returns></returns>
        public static bool WriteFile(string fileFullPath, string fileName, string data)
        {
            //(每隔一个小时创建一个新的文件）
            DateTime now = DateTime.Now;
            string nowFileName = string.Format("{0}_{1:00}_{2:00}_{3:00}",
                fileName, now.Year, now.Month, now.Hour); 
            
            string fileFullName = string.Format("{0}\\{1}.txt", fileFullPath, nowFileName);
            try
            {
                if (false == Directory.Exists(fileFullPath))
                {
                    //创建文件夹
                    Directory.CreateDirectory(fileFullPath);
                }
                using (FileStream file = new FileStream(fileFullName, FileMode.Append, FileAccess.Write, FileShare.Read))
                {
                    data = data + "\r\n";
                    byte[] bytes = Encoding.UTF8.GetBytes(data);

                    file.Write(bytes, 0, bytes.Length);
                    file.Flush();
                    file.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.Instance().Log(LogType.WriteFileFailed,
                    string.Format("{0}\r\n{1}", fileFullName, ex.ToString()));
            }
            
            return false;
        }
    }
}
