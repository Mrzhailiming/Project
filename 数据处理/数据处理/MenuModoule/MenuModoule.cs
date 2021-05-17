using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.IO;

namespace Data
{
    public class MenuModoule
    {
        //停止标志
        bool _exit = false;
        //当前路径
        public string _curFullPath = null;
        //所有文件
        public Dictionary<string, string> _allFiles = new Dictionary<string, string>();
        
        //count 计数
        public UInt32 _counter = 0;

        Thread _thread;

        virtual public void Begin()
        {
            //获取当前路径
            _curFullPath = Directory.GetCurrentDirectory();
            
            _thread = new Thread(Execute);
            _thread.Start();
        }
        virtual public void Execute()
        {
            Console.WriteLine("menu启动成功");
            PrintMenu();
            while (!_exit)
            {
                MyConsole.WriteLine("------------------------------------------------------------------------------------------------------------", ConsoleColor.Blue);
                
                Console.WriteLine("请输入选项：");

                _exit = !ReadChoice();

            }
            Console.WriteLine("menu退出成功");
        }
        
        virtual public void PrintMenu()
        {
            Logger.Instance().Log(LogType.Error, "virtual public void PrintMenu");
            MyConsole.WriteLine("重写 virtual public void PrintMenu", ConsoleColor.Red);
        }
        
        /// <summary>
        /// read用户的选择
        /// </summary>
        virtual public bool ReadChoice()
        {
            Logger.Instance().Log(LogType.Error, "virtual public bool ReadChoice");
            MyConsole.WriteLine("重写 virtual public bool ReadChoice", ConsoleColor.Red);
            return false;
        }
        /// <summary>
        /// 重新read选择
        /// </summary>
        public void ReReadChoice()
        {
            Console.WriteLine("请重新输入：");
            ReadChoice();
        }
        ////这个好像不需要作为虚函数
        //virtual public void Start()
        //{
        //    Logger.Instance().Log(LogType.Error, "virtual public void Start");
        //    MyConsole.WriteLine("重写 virtual public void Start", ConsoleColor.Red);
        //}
        ////这个也是
        //public void Restart()
        //{
        //    //先把状态置为初始状态
        //    //1.读取的数据等等
        //    Reset();
        //    //
        //    Start();
        //}
        /// <summary>
        /// 重置一些数据
        /// </summary>
        virtual public void Reset()
        {
            _exit = false;

            _curFullPath = Directory.GetCurrentDirectory();

            _counter = 0;
        }
        /// <summary>
        /// 退出
        /// </summary>
        virtual public void Exit()
        {
            _exit = true;

        }
        /// <summary>
        /// 刷新缓冲区
        /// </summary>
        virtual public void Flush()
        {
            
        }
    }

    internal enum Choice
    {
        Error,
        Exit = 48,
        Srart = 49,//1
        Restart = 50,//2
        OneFile = 51,//3
        ReOneFile = 52,//4
        InputPath = 53,
    }
}
