using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.IO;

namespace Data
{
    public class Menu
    {
        //停止标志
        bool _exit = false;
        //当前路径
        string _curFullPath = null;
        //所有文件
        Dictionary<string, string> _allFiles = new Dictionary<string, string>();
        //两个峰值
        DateAndTwoFengzhi _manager;
        //count 计数
        UInt32 _counter = 0;

        public void Init()
        {
            Thread th = new Thread(Execute);
            th.Start();
        }
        public void Execute()
        {
            while (!_exit)
            {
                PrintMenu();

                //Flush();
                Console.WriteLine("请输入：");
                ReadChoice();
            }
            Console.WriteLine("退出成功");
        }
        private void Start()
        {
            //获取程序当前路径
            _curFullPath = Directory.GetCurrentDirectory();
            _manager = new DateAndTwoFengzhi(_curFullPath);

            _allFiles = _manager.GetAllFiles();

            //输出一个开始行
            OutPutPeaks(_curFullPath, "<-start->", new UInt32[1]);
            foreach (string fileFullName in _allFiles.Values)
            {

                UInt32[] peaks = _manager.GetPeaks(fileFullName);
                OutPutPeaks(_curFullPath, fileFullName, peaks);
            }
        }
        private void PrintMenu()
        {
            Console.WriteLine("*******************************");
            Console.WriteLine("************1.start************");
            Console.WriteLine("************2.restart**********");
            Console.WriteLine("************3.one file**********");

            Console.WriteLine("************0.exit*************");
            Console.WriteLine("*******************************");

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
            //控制台
            Console.WriteLine("文件:{0}\t峰值a:{1} 峰值b:{2}", fileFullName, peaks[0], peaks[1]);

            //文件
            MyFileStream.WriteFile(_curFullPath + "\\数据", "peaks",
                string.Format("{0} {1}\t{2};\t{3}\t{4}", peaks[0], peaks[1], fileFullName, DateTime.Now.ToString(), _counter++));
        }
        /// <summary>
        /// read用户选择
        /// </summary>
        private bool ReadChoice()
        {
            int ch = 0;
            try
            {
                string str = Console.ReadLine();
                ch = Convert.ToInt32(str[0]);
            }
            catch (Exception ex)
            {
                //第一次输入的读取的为 ""
                ch = (int)Choice.Srart;
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
                case (int)Choice.Exit:
                    Exit();
                    break;
                default:
                    ReReadChoice();
                    break;
            }
            return true;
        }
        
        private void Restart()
        {
            //先把状态置为初始状态
            //1.读取的数据等等
            Reset();
            //
            Start();
        }
        private void Reset()
        {
            //停止标志
            _exit = false;
            //当前路径
            _curFullPath = null;
            //所有文件
            _allFiles.Clear();

            //两个峰值的类
            _manager.Reset();
            //count 计数
            _counter = 0;
        }
        /// <summary>
        /// 重新read选择
        /// </summary>
        private void ReReadChoice()
        {
            Console.WriteLine("输入错误请重新输入：");
            ReadChoice();
        }
        private void OneFile()
        {
            Console.Write("请把文件拖到此处或者输入文件完整路径：");
            string fileFullName = Console.ReadLine();

            UInt32[] peaks = _manager.GetPeaks(fileFullName);
            OutPutPeaks(_curFullPath, fileFullName, peaks); ;
        }
        /// <summary>
        /// 退出
        /// </summary>
        private void Exit()
        {
            _exit = true;

        }
        /// <summary>
        /// 刷新缓冲区
        /// </summary>
        private void Flush()
        {
            while (Console.Read() != -1)
            {
                Console.Read();
            }
        }
    }

    internal enum Choice
    {
        Error,
        Exit = 48,
        Srart = 49,
        Restart = 50,
        OneFile = 51,
    }
}
