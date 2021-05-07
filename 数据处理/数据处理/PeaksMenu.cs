using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    class PeaksMenu : MenuModoule
    {
        //两个峰值
        DateAndTwoFengzhi _manager;


        override public void Init()
        {
 	        base.Init();
            _manager = new DateAndTwoFengzhi(_curFullPath);
        }
        override public void PrintMenu()
        {
            Console.WriteLine("*******************************************************************");
            Console.WriteLine("************1.计算当前程序运行路径下的文件*************************");
            Console.WriteLine("************2.重新计算当前程序运行路径下的文件*********************");
            Console.WriteLine("************3.计算一个指定的文件***********************************");
            Console.WriteLine("*****注意：如果文件不在当前程序的目录下，则需要以管理员身份运行****");

            Console.WriteLine("************4.重新计算一个指定的文件*******************************");
            Console.WriteLine("************5.计算指定路径下的所有文件*****************************");

            Console.WriteLine("************0.退出*************************************************");
            Console.WriteLine("*******************************************************************");
        }
        override public bool ReadChoice()
        {
            int ch = 0;

            string str = Console.ReadLine();
            if (str.Length == 1)
            {
                ch = Convert.ToInt32(str[0]);
            }
            else
            {
                ch = (int)Choice.Error;
            }

            switch (ch)
            {
                case (int)Choice.Srart:
                    Start();
                    break;
                case (int)Choice.Restart:
                    Restart();
                    break;
                case (int)Choice.OneFile:
                    OneFile();
                    break;
                case (int)Choice.ReOneFile:
                    ReOneFile();
                    break;
                case (int)Choice.InputPath:
                    InputPath();
                    break;
                case (int)Choice.Exit:
                    Exit();
                    break;
                default:
                    ReReadChoice();
                    break;
            }
            return true;
        }
        /// <summary>
        /// 扫描程序当前路径文件，并计算
        /// </summary>
        override public void Start()
        {
            _manager.Init();//扫描文件
            _allFiles = _manager.GetAllFiles();
            try
            {
                //输出一个开始行
                OutPutPeaks(_curFullPath, "", new UInt32[0]);
                int count = 0;
                foreach (string fileFullName in _allFiles.Values)
                {

                    UInt32[] peaks = _manager.GetPeaks(fileFullName);
                    if (peaks.Length < 2)
                    {
                        continue;
                    }
                    ++count;
                    OutPutPeaks(_curFullPath, fileFullName, peaks);
                    //控制台
                    Console.WriteLine("文件:{0}\t峰值：{1} {2}", fileFullName, peaks[0], peaks[1]);
                }

                //控制台
                Console.WriteLine("共有文件:{0}个:计算出峰值的文件：{1}个", _allFiles.Count, count);
            }
            catch (Exception ex)
            {
                Logger.Instance().Log(LogType.Error, ex.StackTrace.ToString());
            }

        }
        //override public void Restart()
        //{
        //    base.Restart();
        //}
        /// <summary>
        /// 计算一个文件的峰值
        /// </summary>
        private void OneFile()
        {
            Console.Write("请把文件拖到此处或者输入文件完整路径：");
            string fileFullName = Console.ReadLine();
            UInt32[] peaks = _manager.GetPeaks(fileFullName);
            if (peaks.Length < 2)
            {
                MyConsole.WriteLine("未能计算出峰值", ConsoleColor.Red);
                return;
            }
            OutPutPeaks(_curFullPath, fileFullName, peaks);
        }
        /// <summary>
        /// 重新计算一个文件的峰值
        /// </summary>
        private void ReOneFile()
        {
            Console.Write("请把文件拖到此处或者输入文件完整路径：");
            string fileFullName = Console.ReadLine();
            UInt32[] peaks = _manager.ReGetPeaks(fileFullName);
            if (peaks.Length > 0)
            {
                ReOutPutPeaks(_curFullPath, fileFullName, peaks);
            }
        }
        /// <summary>
        /// 计算指定路径下的所有文件
        /// </summary>
        private void InputPath()
        {
            Console.Write("请输入完整路径：");
            string fileFullPath = Console.ReadLine();
            DateAndTwoFengzhi newMag = new DateAndTwoFengzhi(fileFullPath);
            InputPathStart(newMag);
        }
        private void InputPathStart(DateAndTwoFengzhi _manager)
        {
            _manager.Init();//扫描文件
            _allFiles = _manager.GetAllFiles();
            try
            {
                //输出一个开始行
                OutPutPeaks(_curFullPath, "", new UInt32[0]);
                int count = 0;
                foreach (string fileFullName in _allFiles.Values)
                {

                    UInt32[] peaks = _manager.GetPeaks(fileFullName);
                    if (peaks.Length < 2)
                    {
                        continue;
                    }
                    ++count;
                    OutPutPeaks(_curFullPath, fileFullName, peaks);
                    //控制台
                    Console.WriteLine("文件:{0}\t峰值：{1} {2}", fileFullName, peaks[0], peaks[1]);
                }

                //控制台
                Console.WriteLine("共有文件:{0}个:计算出峰值的文件：{1}个", _allFiles.Count, count);
            }
            catch (Exception ex)
            {
                Logger.Instance().Log(LogType.Error, ex.StackTrace.ToString());
            }

        }
        /// <summary>
        /// 
        /// </summary>
        public override void Reset()
        {
            base.Reset();

            _manager = new DateAndTwoFengzhi(_curFullPath);

            _allFiles = _manager.GetAllFiles();
        }
        /// <summary>
        /// 输出峰值到peaks文件
        /// </summary>
        /// <param name="_curFullPath"></param>
        /// <param name="fileFullName"></param>
        /// <param name="peaks"></param>
        private void OutPutPeaks(string _curFullPath, string fileFullName, UInt32[] peaks)
        {
            if (peaks.Length < 2)
            {
                MyFileStream.WriteFile(_curFullPath + "\\数据", "peaks", string.Format("<-start 峰值-文件名-时间-序号 {0};{1}->", DateTime.Now.ToString(), _counter++));
                return;
            }

            //文件
            MyFileStream.WriteFile(_curFullPath + "\\数据", "peaks",
                string.Format("{0} {1}\t{2};\t{3}\t{4}", peaks[0], peaks[1], fileFullName, DateTime.Now.ToString(), _counter++));
        }
        /// <summary>
        /// 输出峰值到peaks文件
        /// </summary>
        /// <param name="_curFullPath"></param>
        /// <param name="fileFullName"></param>
        /// <param name="peaks"></param>
        private void ReOutPutPeaks(string _curFullPath, string fileFullName, UInt32[] peaks)
        {
            if (peaks.Length < 2)
            {
                MyFileStream.WriteFile(_curFullPath + "\\数据", "RePeaks", string.Format("<-start 峰值-文件名-时间-序号 {0};{1}->", DateTime.Now.ToString(), _counter++));
                return;
            }
            //控制台
            Console.WriteLine("重新计算文件:{0}\t峰值a:{1} 峰值b:{2}", fileFullName, peaks[0], peaks[1]);

            //文件
            MyFileStream.WriteFile(_curFullPath + "\\数据", "RePeaks",
                string.Format("{0} {1}\t{2};\t{3}\t{4}", peaks[0], peaks[1], fileFullName, DateTime.Now.ToString(), _counter++));
        }
    }
}
